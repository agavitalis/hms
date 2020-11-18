using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientPreConsultationViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientPreConsultation
    {
        Task<IEnumerable<PatientPreConsultation>> GetPatientPreConsultation(string PatientId);
        Task<bool> UpdatePatientVitalsAsync(UpdatePatientVitalsViewModel patientVitals);
        Task<bool> UpdatePatientBMIAsync(UpdatePatientBMIViewModel patientBMI);

    }
}
