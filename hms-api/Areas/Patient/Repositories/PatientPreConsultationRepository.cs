using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.Models;
using HMS.Database;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientPreConsultationViewModel;

namespace HMS.Areas.Patient.Repositories
{
    public class PatientPreConsultationRepository : IPatientPreConsultation
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PatientPreConsultationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> UpdatePatientVitalsAsync(UpdatePatientVitalsViewModel PatientVitals)
        {
            //throw new NotImplementedException();
            //check if this guys VItals has been checked this hour

            var PreConsultation = await _applicationDbContext.PatientPreConsultation.FirstOrDefaultAsync(d => d.PatientId == PatientVitals.PatientId && d.Date.Hour == DateTime.Now.Hour);

            // Validate vitals are not null---no vitals this hour
            if (PreConsultation == null)
            {
                var patientPreConsultation = new PatientPreConsultation()
                {
                    BloodPressure = PatientVitals.BloodPressure,
                    Pulse = PatientVitals.Pulse,
                    Respiration = PatientVitals.Respiration,
                    SPO2 = PatientVitals.SPO2,
                    Temperature = PatientVitals.Temperature,
                    PatientId = PatientVitals.PatientId,
                    Date = DateTime.Now
                };
                _applicationDbContext.PatientPreConsultation.Add(patientPreConsultation);
                await _applicationDbContext.SaveChangesAsync();
            }
            else
            {
                PreConsultation.BloodPressure = PatientVitals.BloodPressure;
                PreConsultation.Pulse = PatientVitals.Pulse;
                PreConsultation.Respiration = PatientVitals.Respiration;
                PreConsultation.SPO2 = PatientVitals.SPO2;
                PreConsultation.Temperature = PatientVitals.Temperature;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            

            return true;
        }

        public async Task<bool> UpdatePatientBMIAsync(UpdatePatientBMIViewModel PatientBMI)
        {
            //throw new NotImplementedException();
            //check if this guys VItals has been checked this hour
           
            var PreConsultation = await _applicationDbContext.PatientPreConsultation.FirstOrDefaultAsync(d => d.PatientId == PatientBMI.PatientId && d.Date.Hour == DateTime.Now.Hour);

            // Validate vitals are not null---no vitals this hour
            if (PreConsultation == null)
            {
                var patientPreConsultation = new PatientPreConsultation()
                {
                    Weight = PatientBMI.Weight,
                    Height = PatientBMI.Height,
                    CalculatedBMI = PatientBMI.CalculatedBMI,
                    PatientId = PatientBMI.PatientId,
                    Date = DateTime.Now
                };

                _applicationDbContext.PatientPreConsultation.Add(patientPreConsultation);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                PreConsultation.Weight = PatientBMI.Weight;
                PreConsultation.Height = PatientBMI.Height;
                PreConsultation.CalculatedBMI = PatientBMI.CalculatedBMI;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
