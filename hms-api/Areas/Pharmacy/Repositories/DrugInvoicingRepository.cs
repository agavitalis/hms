using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugInvoicingRepository : IDrugInvoicing
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;
        private readonly IDrugBatch _drugBatch;

        public DrugInvoicingRepository(ApplicationDbContext applicationDbContext, IDrugBatch drugBatch, ITransactionLog transaction, IAccount account)
        {

            _applicationDbContext = applicationDbContext;     
            _transaction = transaction;
            _drugBatch = drugBatch;
            _account = account;
        
        }

        public async Task<bool> CheckIfDrugsExist(List<Drugs> drugs)
        {
            if (drugs == null)
                return false;

            var idNotInDrugs = drugs.Where(x => _applicationDbContext.Drugs.Any(y => y.Id == x.drugId));

            return idNotInDrugs.Any();
        }

        public async Task<string> GenerateDrugDispenseInvoice(DrugInvoicingDto drugInvoicing)
        {
            try
            {
                decimal totalUnitPrice = 0;
                decimal totalContainerPrice = 0;
                decimal totalCartonPrice = 0;
                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
                string priceCalculationFormular = "";

                if (drugInvoicing == null)
                    return null;

             

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugInvoicing.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                //get the drug price based on the health plan above
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Include(h => h.HMOHealthPlan).ThenInclude(h => h.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();




                decimal totalDrugPricing = 0;
                decimal amountDue = 0;
                decimal HMOAmount = 0;
                
               
                foreach (var _drug in drugInvoicing.Drugs)
                {
                    //Check if the drug is in stock
                    var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                    var quantityOfDrugs = _drug.numberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + _drug.numberOfContainers * drug.QuantityPerContainer + _drug.numberOfUnits;
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId && p.DrugId == drug.Id).FirstOrDefaultAsync();
                    
                    var drugBatch = await _drugBatch.GetDrugBatchByDrug(drug.Id, quantityOfDrugs);
                    if (drugBatch == null)
                    {
                        return "1";
                    }

                    
                    if (HMOHealthPlanPatient != null)
                    {
                       var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanDrugPrice != null)
                        {
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                        }
                    }
                    else if (HMOHealthPlanSubGroupPatient != null)
                    {
                        var  HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanDrugPrice != null)
                        {
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
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
                            totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                            priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                        }
                    }
                    else if (drugPrice != null)
                    {
                        if (drugPrice != null)
                        {
                            totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = drugPrice.HealthPlan.Name;
                        }
                    }
                    else
                    {
                        totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                        AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";
                    }





                    if (String.IsNullOrEmpty(_drug.numberOfUnits.ToString()))
                    {
                        _drug.numberOfUnits = 0;
                    }

                    if (String.IsNullOrEmpty(_drug.numberOfContainers.ToString()))
                    {
                        _drug.numberOfContainers = 0;
                    }

                    if (String.IsNullOrEmpty(_drug.numberOfCartons.ToString()))
                    {
                        _drug.numberOfCartons = 0;
                    }

                    

                    
                    

                    totalDrugPricing += priceTotal;
                    amountDue += AmountToBePaidByPatient;
                    HMOAmount += AmountToBePaidByHMO;
                }

                if (HMOHealthPlanPatient != null)
                {
                    var drugInvoice = new DrugDispensingInvoice()
                    {
                        AmountTotal = totalDrugPricing,
                        AmountToBePaidByPatient = amountDue,
                        AmountToBePaidByHMO = HMOAmount,
                        PaymentStatus = "PAID",
                        DatePaid = DateTime.Now,
                        GeneratedBy = drugInvoicing.GeneratedBy,
                        PatientId = drugInvoicing.PatientId,
                        ClerkingId = drugInvoicing.ClarkingId,
                        PriceCalculationFormular = priceCalculationFormular
                    };

                    await _applicationDbContext.DrugDispensingInvoices.AddAsync(drugInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return drugInvoice.Id;
                }
                else
                {
                    var drugDispensingInvoice = new DrugDispensingInvoice()
                    {
                        AmountTotal = totalDrugPricing,
                        AmountToBePaidByPatient = amountDue,
                        AmountToBePaidByHMO = HMOAmount,
                        PaymentStatus = "NOT PAID",
                        GeneratedBy = drugInvoicing.GeneratedBy,
                        PatientId = drugInvoicing.PatientId,
                        ClerkingId = drugInvoicing.ClarkingId,
                        PriceCalculationFormular = priceCalculationFormular
                    };

                    await _applicationDbContext.DrugDispensingInvoices.AddAsync(drugDispensingInvoice);
                    await _applicationDbContext.SaveChangesAsync();

                    return drugDispensingInvoice.Id;
                }

                

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<object> CreateDespenseRequest(DrugInvoicingDto drugInvoicingDto, string invoiceId)
        {
            try
            {
                decimal totalUnitPrice = 0;
                decimal totalContainerPrice = 0;
                decimal totalCartonPrice = 0;
                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
                string priceCalculationFormular = "";

             
                if (drugInvoicingDto == null || string.IsNullOrEmpty(invoiceId))
                    return false;

                //grab the patient complete details
                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugInvoicingDto.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();

                List<object> drugList = new List<object>();
                DrugDispensingInvoice drugInvoice = new DrugDispensingInvoice();

                foreach (var _drug in drugInvoicingDto.Drugs)
                {
                 
                    //Check if the drug is in stock
                    var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                    var quantityOfDrugs = _drug.numberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + _drug.numberOfContainers * drug.QuantityPerContainer + _drug.numberOfUnits;
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId && p.DrugId == drug.Id).FirstOrDefaultAsync();


                    //get the drug price based on the health plan patient belongs to above
                    if (HMOHealthPlanPatient != null)
                    {
                        var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanDrugPrice != null)
                        {
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
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
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlan.Name;
                        }
                    }
                    else if (NHISHealthPlanPatient != null)
                    {
                        var NHISDrug = await _applicationDbContext.NHISHealthPlanDrugs.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (NHISDrug != null)
                        {
                            totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                            priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                        }
                    }
                    else if (drugPrice != null)
                    {
                        if (drugPrice != null)
                        {
                            totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = drugPrice.HealthPlan.Name;
                        }
                    }
                    else
                    {

                        totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                        AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";

                    }




                  


                    //save drugs to dispensing
                    DrugDispensing drugDispensing = new DrugDispensing
                    {
                        DrugId = _drug.drugId,
                        NumberOfCartons = _drug.numberOfCartons,
                        NumberOfContainers = _drug.numberOfContainers,
                        NumberOfUnits = _drug.numberOfUnits,

                        TotalCartonPrice = totalCartonPrice,
                        TotalContainerPrice = totalContainerPrice,
                        TotalUnitPrice = totalUnitPrice,
                        PriceTotal = priceTotal,
                        
                        PriceCalculationFormular = priceCalculationFormular,

                        PaymentStatus = "Not Paid",
                        DrugDispensingInvoiceId = invoiceId,
                        ClerkingId = drugInvoicingDto.ClarkingId
                    };
                    await _applicationDbContext.DrugDispensings.AddAsync(drugDispensing );
                    await _applicationDbContext.SaveChangesAsync();

                    //add everything to a list
                    var drugsListed = new
                    {
                        drug = drugDispensing
                    };

                    drugList.Add(drugsListed);
                    drugInvoice = drugDispensing.DrugDispensingInvoice;
                }

                var drugInvoiceList= new
                {
                    drugs = drugList,
                    invoice = drugInvoice
                };

                return drugInvoiceList;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<IEnumerable<DrugDispensingInvoice>> GetDrugDispencingInvoices()
        {
          return  await _applicationDbContext.DrugDispensingInvoices.OrderByDescending(s=> s.DateGenerated)
                .Include(p=>p.Patient)
                .ToListAsync();
        }

        public async Task<DrugDispensingInvoice> GetDrugDispencingInvoice(string DrugDispensingInvoice) => await _applicationDbContext.DrugDispensingInvoices.Where(p => p.Id == DrugDispensingInvoice).Include(p => p.Patient).FirstOrDefaultAsync();

        public async Task<IEnumerable<DrugDispensingInvoice>> GetPatientDrugInvoices(string patientId)
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(p=>p.PatientId == patientId)
                .OrderByDescending(s => s.DateGenerated)
                .Include(d=>d.DrugDispensing)
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetDrugsInAnInvoice(string invoiceNumber)
        {
            var drugsInInvoice = await _applicationDbContext.DrugDispensings.Include(d => d.Drug).Include(d => d.Clerking.Doctor).Include(d => d.Clerking.Patient).Where(s => s.DrugDispensingInvoice.InvoiceNumber == invoiceNumber)
                .OrderBy(d => d.Drug.Name)
                .ToListAsync();
            return drugsInInvoice;
        }

        public async Task<bool> CheckIfAmountPaidIsCorrect(string AdmissionInvoiceId, decimal amount)
        {
            if (string.IsNullOrEmpty(AdmissionInvoiceId))
                return false;

         
            var drugDispensed = _applicationDbContext.DrugDispensingInvoices.Where(i => i.InvoiceNumber == AdmissionInvoiceId).FirstOrDefault();
            if(drugDispensed != null)
            {
                //check if the ammount tallys
                if(drugDispensed.AmountToBePaidByPatient != amount)
                {
                    return false;
                         
                }

                return true;
            }
            else
            {
                return false;
            }   
                
           
        }

        public async Task<bool> PayForDrugs(DrugInvoicingPaymentDto drugPayment)
        {

            var drugInvoice = await _applicationDbContext.DrugDispensingInvoices.Where(i => i.InvoiceNumber == drugPayment.InvoiceNumber).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.DrugDispensings.Include(d => d.Drug).Where(d => d.DrugDispensingInvoiceId == drugInvoice.Id).ToListAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugPayment.PatientId).FirstOrDefaultAsync();
            string transactionType = "Credit";
            string invoiceType = "Drug";
            DateTime transactionDate = DateTime.Now;
            //ToDO::Check if the drug is in stock and deduct
          
            //mark all drugs as paid
            foreach (var drugs in drugsDispensed)
            {
                var drug = _applicationDbContext.Drugs.Find(drugs.Drug.Id);
                int drugCount = drugs.NumberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + drugs.NumberOfContainers * drug.QuantityPerContainer + drugs.NumberOfUnits;

                var drugBatch = await _drugBatch.GetDrugBatchByDrug(drug.Id, drugCount);
                if (drugBatch == null)
                {
                    return false;
                }
                drugBatch.QuantityInStock -= drugs.NumberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + drugs.NumberOfContainers * drug.QuantityPerContainer + drugs.NumberOfUnits;

                await _drugBatch.UpdateDrugBatch(drugBatch);
                
                var DrugPayment = await _applicationDbContext.DrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                    DrugPayment.PaymentStatus = "PAID";
                    await  _applicationDbContext.SaveChangesAsync();
            }
           
            //mark the invoice imself as paid
            var DrugDispensingInvoice = await _applicationDbContext.DrugDispensingInvoices.FirstOrDefaultAsync(s => s.InvoiceNumber == drugPayment.InvoiceNumber);
            DrugDispensingInvoice.PaymentStatus = "PAID";
            DrugDispensingInvoice.PaymentMethod = drugPayment.PaymentMethod;
            DrugDispensingInvoice.PaymentReference = drugPayment.TransactionReference;
            DrugDispensingInvoice.DatePaid = DateTime.Now;
 
            await _applicationDbContext.SaveChangesAsync();

            await _transaction.LogTransactionAsync(drugPayment.TotalAmount, transactionType, invoiceType, DrugDispensingInvoice.Id, drugPayment.PaymentMethod, transactionDate, patient.Patient.Id, drugPayment.InitiatorId);
            return true;
        }

        public async Task<bool> PayForDrugsWithAccount(DrugInvoicingPaymentDto drugPayment)
        {

            string transactionType = "Credit";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";
        
            string invoiceType = "Drug";
            string paymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var drugInvoice = await _applicationDbContext.DrugDispensingInvoices.Where(i => i.InvoiceNumber == drugPayment.InvoiceNumber).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.DrugDispensings.Where(d => d.DrugDispensingInvoiceId == drugInvoice.Id).ToListAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugPayment.PatientId).Include(p => p.Account).FirstOrDefaultAsync();
            //ToDO::Check if the drug is in stock and deduct

            //mark all drugs as paid
            foreach (var drugs in drugsDispensed)
            {
                var drug = _applicationDbContext.Drugs.Find(drugs.Drug.Id);
                int drugCount = drugs.NumberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + drugs.NumberOfContainers * drug.QuantityPerContainer + drugs.NumberOfUnits;

                var drugBatch = await _drugBatch.GetDrugBatchByDrug(drug.Id, drugCount);
                if (drugBatch == null)
                {
                    return false;
                }
                drugBatch.QuantityInStock -= drugs.NumberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + drugs.NumberOfContainers * drug.QuantityPerContainer + drugs.NumberOfUnits;

                await _drugBatch.UpdateDrugBatch(drugBatch);
                var DrugPayment = await _applicationDbContext.DrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                DrugPayment.PaymentStatus = "PAID";
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark the invoice imself as paid
            var DrugDispensingInvoice = await _applicationDbContext.DrugDispensingInvoices.FirstOrDefaultAsync(s => s.InvoiceNumber == drugPayment.InvoiceNumber);
            DrugDispensingInvoice.PaymentStatus = "PAID";
            DrugDispensingInvoice.PaymentMethod = drugPayment.PaymentMethod;
            DrugDispensingInvoice.PaymentReference = drugPayment.TransactionReference;
            DrugDispensingInvoice.DatePaid = DateTime.Now;
         

            var account = await _applicationDbContext.Accounts.FirstOrDefaultAsync(s => s.Id == patient.AccountId);
            var previousAccountBalance = account.AccountBalance;
            account.AccountBalance -= drugPayment.TotalAmount;
           

            var accountInvoiceToCreate = new AccountInvoice();

            accountInvoiceToCreate = new AccountInvoice()
            {
                Amount = drugPayment.TotalAmount,
                GeneratedBy = drugPayment.InitiatorId,
                PaymentMethod = drugPayment.PaymentMethod,
                TransactionReference = drugPayment.TransactionReference,
                AccountId = account.Id,
            };

            var accountInvoice = await _account.CreateAccountInvoice(accountInvoiceToCreate);

            await _transaction.LogTransactionAsync(drugPayment.TotalAmount, transactionType, invoiceType,  drugInvoice.Id, drugPayment.PaymentMethod, transactionDate, patient.Patient.Id, drugPayment.InitiatorId);
            await _transaction.LogAccountTransactionAsync(drugPayment.TotalAmount, accountTransactionType, accountInvoiceType, accountInvoice.Id, paymentMethod, transactionDate, patient.Account.Id, previousAccountBalance, drugPayment.InitiatorId);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateDrugInvoice(DrugDispensingInvoice invoice)
        {
            try
            {
                if (invoice == null)
                {
                    return false;
                }

                _applicationDbContext.DrugDispensingInvoices.Update(invoice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetPaidDrugInvoiceCount()
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(s => s.PaymentStatus == "PAID").CountAsync();
        }

        public async Task<int> GetUnPaidDrugInvoiceCount()
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(s => s.PaymentStatus == "NOT PAID").CountAsync();
        }

        public async Task<int> GetPaidDrugInvoiceDispensedCount()
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(s => s.IsDispensed == true).CountAsync();
        }

        public async Task<int> GetPaidDrugInvoiceNotDispensedCount()
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(s => s.IsDispensed == false).CountAsync();
        }
    }
}
