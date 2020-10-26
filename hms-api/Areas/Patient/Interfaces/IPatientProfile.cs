using HMS.Models.Patient;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientProfileViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientProfile
    {
        Task<PatientProfile> GetPatientByIdAsync(string patientId);
        Task<object> GetPatientProfileByIdAsync(string patientId);
        Task<bool> EditPatientBasicInfoAsync(EditPatientBasicInfoViewModel patientProfile);

        Task<bool> EditPatientProfilePictureAsync(PatientProfilePictureViewModel patientProfile);
       
        Task<bool> EditPatientAddressAsync(PatientAddressViewModel patientProfile);
        Task<bool> EditPatientHealthAsync(PatientHealthViewModel patientProfile);
        Task<object> GetPatientsAsync();
        Task<dynamic> GetPatientAppointmentByIdAsync(string patientId);

    }
}
