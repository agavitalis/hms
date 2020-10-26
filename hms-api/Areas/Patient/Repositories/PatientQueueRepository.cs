using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models.Patient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Repositories
{
    public class PatientQueueRepository : IPatientQueue
    {
        private readonly ApplicationDbContext _applicationDbContext;
    
        public PatientQueueRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> AddPatientToQueueAsync(AddPatientToQueueViewModel patientQueue)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == patientQueue.PatientId);
            
            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //add patient to queue
                var queue = new PatientQueue()
                {
                    ConsultationTitle = patientQueue.ConsultationTitle ,
                    ReasonForConsultation = patientQueue.ReasonForConsultation,
                    PatientId = patientQueue.PatientId,
                    DoctorId = patientQueue.DoctorId
                };

                
                _applicationDbContext.PatientQueue.Add(queue);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<int> CancelPatientConsultationAsync(string patientQueueId)
        {
            //check if the patient is in queue today
            var PatientQueue = await _applicationDbContext.PatientQueue.FirstOrDefaultAsync(d => d.Id == patientQueueId);


            if (PatientQueue == null)
            {
                return 1;
            }
            else if (PatientQueue.IsExpired == true)
            {
                return 2;
            }
            else if (PatientQueue.IsCompleted == true)
            {
                return 3;
            }
            else
            {
                PatientQueue.IsCanceled = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> CompletePatientConsultationAsync(string patientQueueId)
        {
            //check if the patient is in queue today
            var PatientQueue = await _applicationDbContext.PatientQueue.FirstOrDefaultAsync(d => d.Id == patientQueueId);


            if (PatientQueue == null)
            {
                return 1;
            }
            else if (PatientQueue.IsExpired == true)
            {
                return 2;
            }
            else if (PatientQueue.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                PatientQueue.IsCompleted = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> ExpirePatientConsultationAsync(string patientQueueId)
        {
            //check if the patient is in queue today
            var PatientQueue = await _applicationDbContext.PatientQueue.FirstOrDefaultAsync(d => d.Id == patientQueueId);


            if (PatientQueue == null)
            {
                return 1;
            }
            else if (PatientQueue.IsCompleted == true)
            {
                return 2;
            }
            else if (PatientQueue.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                PatientQueue.IsExpired = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<object> GetPatientQueue()
        {
       
            var PatientQueue = _applicationDbContext.PatientQueue.Where(p => p.DateOfConsultation.Date == DateTime.Today)
                 .Join(
                           _applicationDbContext.ApplicationUsers,
                           PatientQueue => PatientQueue.PatientId,
                           applicationUsers => applicationUsers.Id,
                           (PatientQueue, patient) => new { PatientQueue, patient }
                       )

                        .Join(
                            _applicationDbContext.ApplicationUsers,
                            PatientQueue => PatientQueue.PatientQueue.DoctorId,
                           applicationUsers => applicationUsers.Id,
                            (PatientQueue, doctor) => new { PatientQueue.PatientQueue, PatientQueue.patient, doctor }
                       )

                        .ToListAsync();
            return await PatientQueue;

        }
    }
}
