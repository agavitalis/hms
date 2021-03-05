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

       
        public PagedList<AdmissionServiceRequestDtoForView> GetAdmissionServiceRequests(string InvoiceId, PaginationParameter paginationParameter)
        {
            var serviceRequests = _applicationDbContext.AdmissionServiceRequests.Include(a => a.Service).Where(a => a.AdmissionInvoiceId == InvoiceId).ToList();

            var serviceRequestToReturn = _mapper.Map<IEnumerable<AdmissionServiceRequestDtoForView>>(serviceRequests);

            return PagedList<AdmissionServiceRequestDtoForView>.ToPagedList(serviceRequestToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
