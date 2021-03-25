using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class DrugDispensingRepository : IAdmissionDrugDispensing
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;
        private readonly IMapper _mapper;
        private readonly IDrugBatch _drugBatch;

        public DrugDispensingRepository(ApplicationDbContext applicationDbContext, IDrugBatch drugBatch, ITransactionLog transaction, IAccount account, IMapper mapper, IAdmissionInvoice admissionInvoice)
        {
            _applicationDbContext = applicationDbContext;
            _admissionInvoice = admissionInvoice;
            _account = account;
            _mapper = mapper;
            _drugBatch = drugBatch;

        }

        public async Task<bool> CheckIfAmountPaidIsCorrect(string invoiceId, decimal amount)
        {
            if (string.IsNullOrEmpty(invoiceId))
                return false;


            var drugDispensed = _applicationDbContext.AdmissionInvoices.Where(i => i.Id == invoiceId).FirstOrDefault();
            if (drugDispensed != null)
            {
                //check if the ammount tallys
                if (drugDispensed.Amount != amount)
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

        public Task<bool> CreateDrugDispensing(AdmissionDrugDispensing AdmissionRequest)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetDrugsInAnInvoice(string InvoiceId)
        {
            var drugsInInvoice = await _applicationDbContext.AdmissionDrugDispensings.Include(d => d.AdmissionInvoice).ThenInclude(d => d.Admission).ThenInclude(d => d.Patient).Where(s => s.AdmissionInvoice.Id == InvoiceId)
                .ToListAsync();
            return drugsInInvoice;
        }

        public async Task<bool> UpdateDrugDispensing(MedicationDtoForAdminister AdmissionRequest, AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionRequest == null)
                    return false;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == AdmissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
                var admissionInvoice = await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionRequest.AdmissionId).FirstOrDefaultAsync();

                
                    //get the drug price based on the health plan above
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();
                    var drug = _applicationDbContext.Drugs.Find(AdmissionRequest.DrugId);
                    decimal totalUnitPrice = 0;
                    decimal totalContainerPrice = 0;
                    decimal totalCartonPrice = 0;
                    decimal priceTotal = 0;
                    string priceCalculationFormular = "";

                    if (drugPrice != null)
                    {
                        totalUnitPrice = drugPrice.PricePerUnit * AdmissionRequest.NumberOfUnits;
                        totalContainerPrice = drugPrice.PricePerContainer * AdmissionRequest.NumberOfContainers;
                        totalCartonPrice = drugPrice.PricePerCarton * AdmissionRequest.NumberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = drugPrice.HealthPlan.Name;
                    }
                    else
                    {

                        totalUnitPrice = drug.DefaultPricePerUnit * AdmissionRequest.NumberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * AdmissionRequest.NumberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * AdmissionRequest.NumberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";

                    }


                    //save drugs to dispensing
                    AdmissionDrugDispensing admissionRequest = new AdmissionDrugDispensing
                    {
                        DrugId = AdmissionRequest.DrugId,
                        NumberOfCartons = AdmissionRequest.NumberOfCartons,
                        NumberOfContainers = AdmissionRequest.NumberOfContainers,
                        NumberOfUnits = AdmissionRequest.NumberOfUnits,

                        TotalCartonPrice = totalCartonPrice,
                        TotalContainerPrice = totalContainerPrice,
                        TotalUnitPrice = totalUnitPrice,
                        DrugPriceTotal = priceTotal,

                        DrugPriceCalculationFormular = priceCalculationFormular,

                        AdmissionInvoiceId = admissionInvoice.Id,
                    };
                    await _applicationDbContext.AdmissionDrugDispensings.AddAsync(admissionRequest);
                
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PayForDrugs(DrugDispensingPaymentDto drugPayment)
        {

            var drugInvoice = await _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.AdmissionDrugDispensings.Include(d => d.Drug).Where(d => d.AdmissionInvoice.AdmissionId == drugInvoice.AdmissionId).ToListAsync();
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();
            string transactionType = "Credit";
            string invoiceType = "Admission";
            DateTime transactionDate = DateTime.Now;


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

                var DrugPayment = await _applicationDbContext.AdmissionDrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark the invoice as paid
            var AdmissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(drugPayment.AdmissionId);
           
            
            //DrugDispensingInvoice.PaymentStatus = "PAID";
            //DrugDispensingInvoice.PaymentMethod = drugPayment.PaymentMethod;
            //DrugDispensingInvoice.TransactionReference = drugPayment.TransactionReference;
            //DrugDispensingInvoice.DatePaid = DateTime.Now;

            await _applicationDbContext.SaveChangesAsync();

            await _transaction.LogTransactionAsync(drugPayment.TotalAmount, transactionType, invoiceType, AdmissionInvoice.Id, drugPayment.PaymentMethod, transactionDate, patient.Patient.Id, drugPayment.InitiatorId);
            return true;
        }

        public async Task<bool> PayForDrugsWithAccount(DrugDispensingPaymentDto drugPayment)
        {

            string transactionType = "Credit";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";

            string invoiceType = "Drug";
            string paymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var drugInvoice = await _applicationDbContext.AdmissionInvoices.Where(i => i.AdmissionId == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.AdmissionDrugDispensings.Where(d => d.AdmissionInvoiceId == drugInvoice.Id).ToListAsync();
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == drugPayment.AdmissionId).FirstOrDefaultAsync();
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admission.PatientId).FirstOrDefaultAsync();


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

                var DrugPayment = await _applicationDbContext.AdmissionDrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                await _applicationDbContext.SaveChangesAsync();
            }

            //mark the invoice imself as paid
            //var DrugDispensingInvoice = await _applicationDbContext.DrugDispensingInvoices.FirstOrDefaultAsync(s => s.InvoiceNumber == drugPayment.InvoiceNumber);
            //DrugDispensingInvoice.PaymentStatus = "PAID";
            //DrugDispensingInvoice.PaymentMethod = drugPayment.PaymentMethod;
            //DrugDispensingInvoice.PaymentReference = drugPayment.TransactionReference;
            //DrugDispensingInvoice.DatePaid = DateTime.Now;


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

        public PagedList<AdmissionDrugDispensingDtoForView> GetAdmissionDrugDispensing(string InvoiceId, PaginationParameter paginationParameter)
        {
            var drugDispensing = _applicationDbContext.AdmissionDrugDispensings.Where(a => a.AdmissionInvoiceId == InvoiceId).Include(a => a.Drug).ToList();

            var drugDispensingToReturn = _mapper.Map<IEnumerable<AdmissionDrugDispensingDtoForView>>(drugDispensing);

            return PagedList<AdmissionDrugDispensingDtoForView>.ToPagedList(drugDispensingToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
