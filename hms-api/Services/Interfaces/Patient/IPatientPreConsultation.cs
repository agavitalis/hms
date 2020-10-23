using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Patient.PatientPreConsultationViewModel;

namespace HMS.Services.Interfaces.Patient
{
    public interface IPatientPreConsultation
    {
        Task<bool> UpdatePatientVitalsAsync(UpdatePatientVitalsViewModel patientPatientVitals);
        Task<bool> UpdatePatientBMIAsync(UpdatePatientBMIViewModel patientPatientBMI);
    }
}
