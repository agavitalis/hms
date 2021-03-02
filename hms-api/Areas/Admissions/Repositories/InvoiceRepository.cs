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


        public async Task<bool> CheckIfAmountPaidIsCorrect(AdmissionPaymentDto admission)
        {
         
            var invoice = _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == admission.AdmissionId).FirstOrDefault();        
            if (invoice.Amount != admission.TotalAmount)
            {
                return false;
            }

            return true;
        }


        public async Task<bool> PayForAdmission(AdmissionPaymentDto admissionPayment)
        {

            var drugInvoice = await _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == admissionPayment.AdmissionId).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.AdmissionDrugDispensings.Include(d => d.Drug).Where(d => d.AdmissionInvoice.AdmissionId == drugInvoice.AdmissionId).ToListAsync();
            var servicesRequested = await _applicationDbContext.AdmissionServiceRequests.Include(a => a.Service).Where(d => d.AdmissionInvoice.AdmissionId == drugInvoice.AdmissionId).ToListAsync();
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == admissionPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();
            string transactionType = "Credit";
            string invoiceType = "Admission";
            DateTime transactionDate = DateTime.Now;


            //mark all services as paid
            foreach (var serviceRequest in servicesRequested)
            {
                var ServicePayment = await _applicationDbContext.AdmissionServiceRequests.FirstOrDefaultAsync(s => s.Id == serviceRequest.Id);
                ServicePayment.PaymentStatus = "PAID";
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark all drugs as paid
            foreach (var drugs in drugsDispensed)
            {
                var DrugPayment = await _applicationDbContext.AdmissionDrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                DrugPayment.PaymentStatus = "PAID";
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark the invoice as paid
            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.AdmissionId == admissionPayment.AdmissionId);
            AdmissionInvoice.PaymentStatus = "PAID";
            AdmissionInvoice.PaymentMethod = admissionPayment.PaymentMethod;
            AdmissionInvoice.TransactionReference = admissionPayment.TransactionReference;
            AdmissionInvoice.DatePaid = DateTime.Now;

            await _applicationDbContext.SaveChangesAsync();

            await _transaction.LogTransactionAsync(admissionPayment.TotalAmount, transactionType, invoiceType, AdmissionInvoice.Id, admissionPayment.PaymentMethod, transactionDate, patient.Patient.Id, admissionPayment.InitiatorId);
            return true;
        }

        public async Task<bool> PayForAdmissionWithAccount(AdmissionPaymentDto drugPayment)
        {

            string transactionType = "Credit";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";

            string invoiceType = "Drug";
            string paymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var drugInvoice = await _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.AdmissionDrugDispensings.Where(d => d.AdmissionInvoiceId == drugInvoice.Id).ToListAsync();
            var servicesRequested = await _applicationDbContext.AdmissionServiceRequests.Include(a => a.Service).Where(d => d.AdmissionInvoice.AdmissionId == drugInvoice.AdmissionId).ToListAsync();
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();

            if (patient.Account.AccountBalance < drugInvoice.Amount)
            {
                return false;
            }

            //mark all services as paid
            foreach (var serviceRequest in servicesRequested)
            {
                var ServicePayment = await _applicationDbContext.AdmissionServiceRequests.FirstOrDefaultAsync(s => s.Id == serviceRequest.Id);
                ServicePayment.PaymentStatus = "PAID";
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark all drugs as paid
            foreach (var drugs in drugsDispensed)
            {
                var DrugPayment = await _applicationDbContext.AdmissionDrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                DrugPayment.PaymentStatus = "PAID";
                await _applicationDbContext.SaveChangesAsync();
            }
            
            //mark the invoice imself as paid
            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.AdmissionId == drugPayment.AdmissionId);
            AdmissionInvoice.PaymentStatus = "PAID";
            AdmissionInvoice.PaymentMethod = drugPayment.PaymentMethod;
            AdmissionInvoice.TransactionReference = drugPayment.TransactionReference;
            AdmissionInvoice.DatePaid = DateTime.Now;


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

            await _transaction.LogTransactionAsync(drugPayment.TotalAmount, transactionType, invoiceType, drugInvoice.Id, drugPayment.PaymentMethod, transactionDate, patient.Patient.Id, drugPayment.InitiatorId);
            await _transaction.LogAccountTransactionAsync(drugPayment.TotalAmount, accountTransactionType, accountInvoiceType, accountInvoice.Id, paymentMethod, transactionDate, patient.Account.Id, previousAccountBalance, drugPayment.InitiatorId);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
