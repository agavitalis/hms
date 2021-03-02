using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
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
    public class ServiceRequestRepository : IAdmissionServiceRequest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPatientProfile _patient;
        private readonly IMapper _mapper;
        private readonly ITransactionLog _transaction;

        public ServiceRequestRepository(ApplicationDbContext applicationDbContext, IPatientProfile patient, ITransactionLog transaction, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _patient = patient;
            _transaction = transaction;
            _mapper = mapper;
    
        }

        public async Task<AdmissionServiceRequest> GetServiceRequest(string serviceRequestId) => await _applicationDbContext.AdmissionServiceRequests.Where(s => s.Id == serviceRequestId).Include(s => s.AdmissionInvoice).Include(s => s.Service).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync();


        public async Task<bool> CreateAdmissionRequest(AdmissionServiceRequest AdmissionServiceRequest)
        {
            try
            {
                if (AdmissionServiceRequest == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionServiceRequests.Add(AdmissionServiceRequest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAdmissionServiceRequest(AdmissionServiceRequestDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionRequest == null)
                    return false;




                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == AdmissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
                var admissionInvoice = await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionRequest.AdmissionId).FirstOrDefaultAsync();
                if (AdmissionRequest.ServiceId != null)
                {
                    AdmissionRequest.ServiceId.ForEach(x =>

                   _applicationDbContext.AdmissionServiceRequests.AddAsync(
                       new AdmissionServiceRequest
                       {
                           ServiceId = x,
                           Amount = _applicationDbContext.Services.Where(s => s.Id == x).FirstOrDefault().Cost,
                           PaymentStatus = "False",
                           AdmissionInvoiceId = admissionInvoice.Id
                       })
                  );
                }

                
                

                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfServiceRequestIdExist(List<string> serviceRequestId)
        {
            if (serviceRequestId == null)
                return false;

            var idNotInServiceRequests = serviceRequestId.Where(x => _applicationDbContext.AdmissionServiceRequests.Any(y => y.Id == x));
            return idNotInServiceRequests.Any();
        }

        public async Task<bool> CheckIfAmountPaidIsCorrect(AdmissionServiceRequestPaymentDto services)
        {
            if (services == null)
                return false;

            decimal amountTotal = 0;
            //check if the amount tallies
            services.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var service = _applicationDbContext.AdmissionServiceRequests.Where(i => i.Id == serviceRequestId).FirstOrDefault();
                amountTotal = amountTotal + service.Amount;

            });

            if (amountTotal != services.TotalAmount)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> PayForServices(AdmissionServiceRequestPaymentDto serviceRequest)
        {
            int servicesPaid = 0;
            string admissionInvoiceId = "";
            string transactionType = "Credit";
            string invoiceType = "Admission";
            DateTime transactionDate = DateTime.Now;
            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == serviceRequest.AdmissionId).FirstOrDefaultAsync();
            serviceRequest.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.AdmissionServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                ServiceRequest.PaymentStatus = "PAID";

                admissionInvoiceId = ServiceRequest.AdmissionInvoiceId;
                servicesPaid++;
                _applicationDbContext.AdmissionServiceRequests.Update(ServiceRequest);

            });

            //log transactions
            serviceRequest.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var AdmissionServiceRequest = _applicationDbContext.AdmissionServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                _transaction.LogTransaction(AdmissionServiceRequest.Amount, transactionType, invoiceType, AdmissionServiceRequest.AdmissionInvoiceId, serviceRequest.PaymentMethod, transactionDate, admission.Patient.Id, serviceRequest.InitiatorId);

            });

            await _applicationDbContext.SaveChangesAsync();

            //now check of all the services in this invoice was paid for
            var serviceCount = await _applicationDbContext.AdmissionServiceRequests.Where(s => s.AdmissionInvoiceId == admissionInvoiceId).CountAsync();

            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.Id == admissionInvoiceId);

            //if (serviceCount == servicesPaid)
            //{
            //    AdmissionInvoice.PaymentStatus = "PAID";
            //    AdmissionInvoice.PaymentMethod = serviceRequest.PaymentMethod;
            //    AdmissionInvoice.DatePaid = DateTime.Now;
            //}
            //else
            //{
            //    AdmissionInvoice.PaymentStatus = "INCOMPLETE";
            //}
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PayForServicesWithAccount(AdmissionServiceRequestPaymentDto serviceRequest)
        {
            int servicesPaid = 0;
            string admissionInvoiceId = "";
            string transactionType = "Credit";
            string invoiceType = "Admission";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";
            string accountInvoiceId = null;
            decimal totalAmount = 0;
            string accountPaymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var admission = await _applicationDbContext.Admissions.Where(a => a.Id == serviceRequest.AdmissionId).Include(a => a.Patient).FirstOrDefaultAsync();
            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);
            serviceRequest.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.AdmissionServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                totalAmount += ServiceRequest.Amount;
            });
            if (patient.Account.AccountBalance < totalAmount)
            {
                return false;
            }

            serviceRequest.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.AdmissionServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                ServiceRequest.PaymentStatus = "PAID";
                admissionInvoiceId = ServiceRequest.AdmissionInvoiceId;

                var account = _applicationDbContext.Accounts.FirstOrDefault(s => s.Id == patient.AccountId);


                var accountInvoiceToCreate = new AccountInvoice();

                accountInvoiceToCreate = new AccountInvoice()
                {
                    Amount = totalAmount,
                    GeneratedBy = serviceRequest.InitiatorId,
                    PaymentMethod = serviceRequest.PaymentMethod,
                    TransactionReference = serviceRequest.TransactionReference,
                    AccountId = account.Id,
                };



                _applicationDbContext.AccountInvoices.Add(accountInvoiceToCreate);
                _applicationDbContext.SaveChanges();
                accountInvoiceId = accountInvoiceToCreate.Id;
                servicesPaid++;
            });

            var account = await _applicationDbContext.Accounts.FirstOrDefaultAsync(s => s.Id == patient.AccountId);
            account.AccountBalance -= totalAmount;
            _applicationDbContext.SaveChanges();

            serviceRequest.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.AdmissionServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                _transaction.LogTransaction(ServiceRequest.Amount, transactionType, invoiceType, ServiceRequest.AdmissionInvoiceId, serviceRequest.PaymentMethod, transactionDate, patient.Patient.Id, serviceRequest.InitiatorId);
                _transaction.LogAccountTransaction(ServiceRequest.Amount, accountTransactionType, accountInvoiceType, accountInvoiceId, accountPaymentMethod, transactionDate, patient.Account.Id, serviceRequest.InitiatorId);
            });


            //now check of all the servies in this invoice was paid for
            var serviceCount = await _applicationDbContext.AdmissionServiceRequests.Where(s => s.AdmissionInvoiceId == admissionInvoiceId).CountAsync();

            var AdmissionInvoice = await _applicationDbContext.AdmissionInvoices.FirstOrDefaultAsync(s => s.Id == admissionInvoiceId);

            if (serviceCount == servicesPaid)
            {
                AdmissionInvoice.PaymentStatus = "PAID";
                AdmissionInvoice.PaymentMethod = serviceRequest.PaymentMethod;
                AdmissionInvoice.DatePaid = DateTime.Now;
            }
            else
            {
                AdmissionInvoice.PaymentStatus = "INCOMPLETE";
            }

            await _applicationDbContext.SaveChangesAsync();

            return true;

        }

        public PagedList<AdmissionServiceRequestDtoForView> GetAdmissionServiceRequests(string InvoiceId, PaginationParameter paginationParameter)
        {
            var serviceRequests = _applicationDbContext.AdmissionServiceRequests.Where(a => a.AdmissionInvoiceId == InvoiceId).ToList();

            var serviceRequestToReturn = _mapper.Map<IEnumerable<AdmissionServiceRequestDtoForView>>(serviceRequests);

            return PagedList<AdmissionServiceRequestDtoForView>.ToPagedList(serviceRequestToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
