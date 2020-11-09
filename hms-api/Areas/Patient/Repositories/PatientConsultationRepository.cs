using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Repositories
{
    public class PatientConsultationRepository : IPatientConsultation
    {
        private readonly ApplicationDbContext _applicationDbContext;
    
        public PatientConsultationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> BookConsultation(BookConsultation consultation)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == consultation.PatientId);
            
            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //add patient to queue
                var queue = new Consultation()
                {
                    ConsultationTitle = consultation.ConsultationTitle ,
                    ReasonForConsultation = consultation.ReasonForConsultation,
                    PatientId = consultation.PatientId,
                    DoctorId = consultation.DoctorId
                };

                
                _applicationDbContext.Consultations.Add(queue);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<int> CancelPatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsExpired == true)
            {
                return 2;
            }
            else if (Consultation.IsCompleted == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsCanceled = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> CompletePatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsExpired == true)
            {
                return 2;
            }
            else if (Consultation.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsCompleted = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> ExpirePatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsCompleted == true)
            {
                return 2;
            }
            else if (Consultation.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsExpired = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<object> GetAPatientConsultations(string patientId)
        {
       
            var Consultation = _applicationDbContext.Consultations.Include(c => c.Patient).Include(c => c.Doctor).ToListAsync();
            return await Consultation;

        }

        public async Task<int> GetCanceledConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCanceled == true).CountAsync();

        public async Task<int> GetCompletedConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCompleted == true).CountAsync();

        public async Task<int> GetPendingConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCompleted == false).CountAsync();
    }
}
