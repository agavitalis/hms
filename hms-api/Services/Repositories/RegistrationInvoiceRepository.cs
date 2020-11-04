using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class RegistrationInvoiceRepository : IRegistrationInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IRegistrationInvoice _invoice;
        private readonly ITransactionLog _transaction;

        public RegistrationInvoiceRepository(ApplicationDbContext applicationDbContext, IMapper mapper, ITransactionLog transaction)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _transaction = transaction;
        }

        public async Task<bool> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy, string patientId)
        {
            var invoiceToGenerate = new RegistrationInvoice()
            {
                Amount = amount,
                HealthPlanId = healthPlanId,
                GeneratedBy = generatedBy,
                PatientId = patientId
            };

            _applicationDbContext.RegistrationInvoices.Add(invoiceToGenerate);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetPatientRegistrationInvoice(string patientId)
        {
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.PatientId == patientId).Include(i => i.HealthPlan).FirstOrDefaultAsync();
            return _mapper.Map<DtoForPatientRegistrationInvoice>(invoice);
        }

        public async Task<bool> PayRegistrationFee(DtoForPatientRegistrationPayment paymentDetails)
        {
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.InvoiceNumber == paymentDetails.InvoiceNumber).FirstOrDefaultAsync();
            if (invoice.Amount != paymentDetails.Amount)
            {
                return false;
            }

            var invoiceToUpdate = _mapper.Map<RegistrationInvoice>(invoice);
            invoiceToUpdate.Amount = paymentDetails.Amount;
            invoiceToUpdate.Description = paymentDetails.Description;
            invoiceToUpdate.ModeOfPayment = paymentDetails.ModeOfPayment;
            invoiceToUpdate.ReferenceNumber = paymentDetails.ReferenceNumber;
            invoiceToUpdate.DatePaid = DateTime.Now;
            invoiceToUpdate.ReferenceNumber = paymentDetails.ReferenceNumber;
            invoiceToUpdate.PaymentStatus = true;
            
            var res = await UpdateRegistrationInvoice(invoiceToUpdate, paymentDetails.Description);
            if (res)
            {
                return true;
            }
            return false;

        }


        public async Task<bool> UpdateRegistrationInvoice(RegistrationInvoice invoice, string description)
        {
            try
            {
                string transactionType = "Credit";
                string invoiceType = "RegistrationInvoice";
                DateTime transactionDate = DateTime.Now;
                if (invoice == null)
                {
                    return false;
                }

                _applicationDbContext.RegistrationInvoices.Update(invoice);
                var res = await _applicationDbContext.SaveChangesAsync();
                if (res == 1)
                {

                    await _transaction.LogTransaction(invoice.Amount, transactionType, invoiceType, invoice.Id, description, transactionDate);
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
