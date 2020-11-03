using HMS.Areas.Patient.Dtos;
using HMS.Areas.Patient.ViewModels;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        IEnumerable<PatientDtoForView> SearchPatient(string searchParam);
        
    }
}
