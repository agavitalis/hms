using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class InvoiceRepository : IAdmissionInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;
        private readonly IDrugBatch _drugBatch;


        public InvoiceRepository(ApplicationDbContext applicationDbContext, ITransactionLog transaction, IDrugBatch drugBatch, IAccount account)
        {
            _applicationDbContext = applicationDbContext;
            _transaction = transaction;
            _account = account;
            _drugBatch = drugBatch;
        }
        
        public async Task<string> CreateAdmissionInvoice(AdmissionInvoice AdmissionInvoice)
        {
            try
            {

                if (AdmissionInvoice == null)
                {
                    return "";
                }

                var admission = await _applicationDbContext.Admissions.Where(a => a.Id == AdmissionInvoice.AdmissionId).FirstOrDefaultAsync();
                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                //get the drug price based on the health plan above
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Include(h => h.HMOHealthPlan).ThenInclude(h => h.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();
                string priceCalculationFormular = "";

                if (HMOHealthPlanPatient != null)
                {
                    priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                    
                }
                else if (HMOHealthPlanSubGroupPatient != null)
                {
                    priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                }
                else if (NHISHealthPlanPatient != null)
                {
                    priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                }
                else if (healthplanId != null)
                {   
                    priceCalculationFormular = PatientProfile.Account.HealthPlan.Name;
                }
                else
                {
                    priceCalculationFormular = "Default Price";
                }

                AdmissionInvoice.PriceCalculationFormula = priceCalculationFormular;
                _applicationDbContext.AdmissionInvoices.Add(AdmissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return AdmissionInvoice.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
        public async Task<AdmissionInvoice> GetAdmissionInvoiceByAdmissionId(string AdmissionId) => await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionId).FirstOrDefaultAsync();


      
      

        public async Task<bool> CheckIfAmountPaidIsCorrect(AdmissionPaymentDto admissionPayment)
        {
            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.AdmissionId == admissionPayment.AdmissionId);
            if (AdmissionInvoice.AmountPaid + admissionPayment.Amount > AdmissionInvoice.Amount)
            {
                return false;
            }
            return true;
        }


        public async Task<bool> PayForAdmission(AdmissionPaymentDto admissionPayment)
        {

            var drugInvoice = await _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == admissionPayment.AdmissionId).FirstOrDefaultAsync();
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == admissionPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();
            string transactionType = "Credit";
            string invoiceType = "Admission";
            DateTime transactionDate = DateTime.Now;

            //mark the invoice as paid
            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.AdmissionId == admissionPayment.AdmissionId);
            AdmissionInvoice.PaymentMethod = admissionPayment.PaymentMethod;
            AdmissionInvoice.AmountPaid += admissionPayment.Amount;
            AdmissionInvoice.TransactionReference = admissionPayment.TransactionReference;
            AdmissionInvoice.DatePaid = DateTime.Now;


            if (AdmissionInvoice.AmountPaid >= AdmissionInvoice.Amount)
            {
                AdmissionInvoice.PaymentStatus = "PAID";
            }
            else
            {
                AdmissionInvoice.PaymentStatus = "INCOMPLETE";
            }

            await _applicationDbContext.SaveChangesAsync();

            await _transaction.LogAdmissionTransactionAsync(admissionPayment.Amount, transactionType, invoiceType, AdmissionInvoice.Id, admissionPayment.PaymentMethod, transactionDate, admissionPayment.AdmissionId, admissionPayment.InitiatorId);
            return true;
        }

        public async Task<bool> PayForAdmissionWithAccount(AdmissionPaymentDto admissionPayment)
        {
            string transactionType = "Credit";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";

            string invoiceType = "Admission";
            string paymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == admissionPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();

            if (patient.Account.AccountBalance < admissionPayment.Amount)
            {
                return false;
            }
            
            //mark the invoice imself as paid
            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.AdmissionId == admissionPayment.AdmissionId);
          
            AdmissionInvoice.PaymentMethod = admissionPayment.PaymentMethod;
            AdmissionInvoice.AmountPaid += admissionPayment.Amount;
            AdmissionInvoice.TransactionReference = admissionPayment.TransactionReference;
            AdmissionInvoice.DatePaid = DateTime.Now;

            if (AdmissionInvoice.AmountPaid >= AdmissionInvoice.Amount)
            {
                AdmissionInvoice.PaymentStatus = "PAID";
            }
            else
            {
                AdmissionInvoice.PaymentStatus = "INCOMPLETE";
            }


            var account = await _applicationDbContext.Accounts.FirstOrDefaultAsync(s => s.Id == patient.AccountId);
            var previousAccountBalance = account.AccountBalance;
            account.AccountBalance -= admissionPayment.Amount;


            var accountInvoiceToCreate = new AccountInvoice();

            accountInvoiceToCreate = new AccountInvoice()
            {
                Amount = admissionPayment.Amount,
                GeneratedBy = admissionPayment.InitiatorId,
                PaymentMethod = admissionPayment.PaymentMethod,
                TransactionReference = admissionPayment.TransactionReference,
                AccountId = account.Id,
            };

            var accountInvoice = await _account.CreateAccountInvoice(accountInvoiceToCreate);

            await _transaction.LogAdmissionTransactionAsync(admissionPayment.Amount, transactionType, invoiceType, AdmissionInvoice.Id, admissionPayment.PaymentMethod, transactionDate, admissionPayment.AdmissionId, admissionPayment.InitiatorId);
            await _transaction.LogAccountTransactionAsync(admissionPayment.Amount, accountTransactionType, accountInvoiceType, accountInvoice.Id, paymentMethod, transactionDate, patient.Account.Id, previousAccountBalance, admissionPayment.InitiatorId);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<string> UpdateAdmissionInvoice(DrugMedicationDtoForAdminister AdmissionRequest, AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionRequest == null)
                    return null;

                decimal totalUnitPrice = 0;
                decimal totalContainerPrice = 0;
                decimal totalCartonPrice = 0;
                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
                string priceCalculationFormular = "";




                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == AdmissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;


                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Include(h => h.HMOHealthPlan).ThenInclude(h => h.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();




                decimal totalDrugPricing = 0;
                decimal amountDue = 0;
                decimal HMOAmount = 0;






                //Check if the drug is in stock
                var drug = _applicationDbContext.Drugs.Find(AdmissionRequest.DrugId);

                int drugCount = AdmissionRequest.NumberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + AdmissionRequest.NumberOfContainers * drug.QuantityPerContainer + AdmissionRequest.NumberOfUnits;
                var drugBatch = await _drugBatch.GetDrugBatchByDrug(drug.Id, drugCount);
                if (drugBatch == null)
                {
                    return "1";
                }

                //get the drug price based on the health plan above

                var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                if (HMOHealthPlanPatient != null)
                {
                    var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                    if (HMOHealthPlanDrugPrice != null)
                    {
                        totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * AdmissionRequest.NumberOfUnits;
                        totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * AdmissionRequest.NumberOfContainers;
                        totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * AdmissionRequest.NumberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                    }
                }
                else if (HMOHealthPlanSubGroupPatient != null)
                {
                    var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                    if (HMOHealthPlanDrugPrice != null)
                    {
                        totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * AdmissionRequest.NumberOfUnits;
                        totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * AdmissionRequest.NumberOfContainers;
                        totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * AdmissionRequest.NumberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                    }
                }
                else if (NHISHealthPlanPatient != null)
                {
                    var NHISDrug = await _applicationDbContext.NHISHealthPlanDrugs.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                    if (NHISDrug != null)
                    {
                        totalUnitPrice = drug.DefaultPricePerUnit * AdmissionRequest.NumberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * AdmissionRequest.NumberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * AdmissionRequest.NumberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                        priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                    }
                }
                else if (drugPrice != null)
                {

                    totalUnitPrice = drugPrice.PricePerUnit * AdmissionRequest.NumberOfUnits;
                    totalContainerPrice = drugPrice.PricePerContainer * AdmissionRequest.NumberOfContainers;
                    totalCartonPrice = drugPrice.PricePerCarton * AdmissionRequest.NumberOfCartons;
                    priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                    AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                    priceCalculationFormular = drugPrice.HealthPlan.Name;
                }
                else
                {
                    totalUnitPrice = drug.DefaultPricePerUnit * AdmissionRequest.NumberOfUnits;
                    totalContainerPrice = drug.DefaultPricePerContainer * AdmissionRequest.NumberOfContainers;
                    totalCartonPrice = drug.DefaultPricePerCarton * AdmissionRequest.NumberOfCartons;
                    AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                    priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                    priceCalculationFormular = "Default Price";
                }
                




                if (String.IsNullOrEmpty(AdmissionRequest.NumberOfUnits.ToString()))
                {
                    AdmissionRequest.NumberOfUnits = 0;
                }

                if (String.IsNullOrEmpty(AdmissionRequest.NumberOfContainers.ToString()))
                {
                    AdmissionRequest.NumberOfContainers = 0;
                }

                if (String.IsNullOrEmpty(AdmissionRequest.NumberOfCartons.ToString()))
                {
                    AdmissionRequest.NumberOfCartons = 0;
                }

                totalDrugPricing += priceTotal;
                amountDue += AmountToBePaidByPatient;
                HMOAmount += AmountToBePaidByHMO;

                if (HMOHealthPlanPatient != null)
                {
                    AdmissionInvoice.Amount += totalDrugPricing;
                    AdmissionInvoice.AmountToBePaidByHMO += HMOAmount;
                    AdmissionInvoice.AmountToBePaidByPatient += amountDue;
                    AdmissionInvoice.PaymentStatus = "Awaiting HMO Payment";



                    _applicationDbContext.AdmissionInvoices.Update(AdmissionInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return AdmissionInvoice.Id;
                }
                else
                {
                    AdmissionInvoice.Amount += totalDrugPricing;
                    AdmissionInvoice.AmountToBePaidByHMO += HMOAmount;
                    AdmissionInvoice.AmountToBePaidByPatient += amountDue;
                    AdmissionInvoice.PaymentStatus = "Not Paid";



                    _applicationDbContext.AdmissionInvoices.Update(AdmissionInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return AdmissionInvoice.Id;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UpdateAdmissionInvoice(ServiceMedicationDtoForAdminister AdmissionRequest, AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                
                if (AdmissionRequest == null)
                    return null;

                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
           
                decimal servicePricing = 0;



                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == AdmissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;


                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Include(h => h.HMOHealthPlan).ThenInclude(h => h.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();




                
                decimal amountDue = 0;
                decimal HMOAmount = 0;






                //Check if the drug is in stock
                var service = _applicationDbContext.Services.Find(AdmissionRequest.ServiceId);

                if (HMOHealthPlanPatient != null)
                {
                    var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.ServiceId == AdmissionRequest.ServiceId).FirstOrDefaultAsync();

                    if (HMOHealthPlanServicePrice != null)
                    {

                        priceTotal = HMOHealthPlanServicePrice.Price;
                        AmountToBePaidByPatient = 0;
                       
                    }
                }
                else if (HMOHealthPlanSubGroupPatient != null)
                {
                    var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.ServiceId == AdmissionRequest.ServiceId).FirstOrDefaultAsync();

                    if (HMOHealthPlanServicePrice != null)
                    {
                        priceTotal = HMOHealthPlanServicePrice.Price;
                        AmountToBePaidByPatient = 0;
                        
                    }
                }
                else if (NHISHealthPlanPatient != null)
                {
                    var NHISService = await _applicationDbContext.NHISHealthPlanServices.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.ServiceId == AdmissionRequest.ServiceId).FirstOrDefaultAsync();

                    if (NHISService != null)
                    {
                        priceTotal = service.Cost;
                        AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                    }
                }
               
                else
                {
                    priceTotal = service.Cost;
                    AmountToBePaidByPatient = service.Cost;
                }



                if (HMOHealthPlanPatient != null)
                {
                    AdmissionInvoice.Amount += priceTotal;
                    AdmissionInvoice.AmountToBePaidByHMO += HMOAmount;
                    AdmissionInvoice.AmountToBePaidByPatient += amountDue;
                    AdmissionInvoice.PaymentStatus = "Awaiting HMO Payment";



                    _applicationDbContext.AdmissionInvoices.Update(AdmissionInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return AdmissionInvoice.Id;
                }
                else
                {
                    AdmissionInvoice.Amount += priceTotal;
                    AdmissionInvoice.AmountToBePaidByHMO += HMOAmount;
                    AdmissionInvoice.AmountToBePaidByPatient += amountDue;
                    AdmissionInvoice.PaymentStatus = "Not Paid";



                    _applicationDbContext.AdmissionInvoices.Update(AdmissionInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return AdmissionInvoice.Id;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAdmissionInvoice(AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionInvoice == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionInvoices.Update(AdmissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
