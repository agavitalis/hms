using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class AdmissionServiceRequestRepository : IAdmissionServiceRequest
    {
        private readonly ApplicationDbContext _applicationDbContext;
     
        public AdmissionServiceRequestRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
    
        }

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
                           ServiceAmount = _applicationDbContext.Services.Where(s => s.Id == x).FirstOrDefault().Cost,
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
    }
}
