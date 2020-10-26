using HMS.Areas.Patient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientPreConsultationViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientPreConsultation
    {
        Task<bool> UpdatePatientVitalsAsync(UpdatePatientVitalsViewModel patientVitals);
        Task<bool> UpdatePatientBMIAsync(UpdatePatientBMIViewModel patientBMI);

    }
}
