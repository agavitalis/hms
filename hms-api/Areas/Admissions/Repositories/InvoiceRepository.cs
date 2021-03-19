using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class InvoiceRepository : IAdmissionInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;


        public InvoiceRepository(ApplicationDbContext applicationDbContext, ITransactionLog transaction, IAccount account)
        {
            _applicationDbContext = applicationDbContext;
            _transaction = transaction;
            _account = account;

        }
        public async Task<string> CreateAdmissionInvoice(AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionInvoice == null)
                {
                    return "";
                }

                _applicationDbContext.AdmissionInvoices.Add(AdmissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return AdmissionInvoice.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AdmissionInvoice> GetAdmissionInvoice(string AdmissionInvoiceId) =>  await _applicationDbContext.AdmissionInvoices.Where(a => a.Id == AdmissionInvoiceId).FirstOrDefaultAsync();

        public async Task<AdmissionInvoice> GetAdmissionInvoiceByAdmissionId(string AdmissionId) => await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionId).FirstOrDefaultAsync();


        public async Task<string> UpdateAdmissionInvoice(AdmissionServiceRequestDtoForCreate AdmissionRequest, AdmissionInvoice admissionInvoice)
        {
            try
            {
                decimal totalDrugPricing = 0;
                if (AdmissionRequest == null)
                    return null;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                List<Service> services = new List<Service>();
                if (AdmissionRequest.ServiceId != null)
                {
                    
                    foreach (var id in AdmissionRequest.ServiceId)
                    {
                        services.Add(_applicationDbContext.Services.Find(id));
                    }
                }

                admissionInvoice.Amount += services.Sum(x => x.Cost);
                admissionInvoice.PaymentStatus = "NOT PAID";


                _applicationDbContext.AdmissionInvoices.Update(admissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return admissionInvoice.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UpdateAdmissionInvoice(AdmissionDrugDispensingDtoForCreate AdmissionRequest, AdmissionInvoice admissionInvoice)
        {
            try
            {
                decimal totalDrugPricing = 0;
                if (AdmissionRequest == null)
                    return null;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                




                if (AdmissionRequest.Drugs != null)
                {
                    foreach (var _drug in AdmissionRequest.Drugs)
                    {
                        //Check if the drug is in stock
                        var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                        if (drug.QuantityInStock < _drug.numberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + _drug.numberOfContainers * drug.QuantityPerContainer + _drug.numberOfUnits)
                        {
                            return "1";
                        }

                        //get the drug price based on the health plan above
                        var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();

                        decimal totalUnitPrice = 0;
                        decimal totalContainerPrice = 0;
                        decimal totalCartonPrice = 0;
                        decimal priceTotal = 0;
                        string priceCalculationFormular = "";

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

                        if (drugPrice != null)
                        {
                            totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = drugPrice.HealthPlan.Name;
                        }
                        else
                        {

                            totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = "Default Price";

                        }

                        totalDrugPricing += priceTotal;

                    }


                }

                admissionInvoice.Amount += totalDrugPricing;
                admissionInvoice.PaymentStatus = "NOT PAID";



                _applicationDbContext.AdmissionInvoices.Update(admissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return admissionInvoice.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


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
    }
}
