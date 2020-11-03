using HMS.Areas.Patient.ViewModels;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientProfile
    {
        Task<PatientProfile> GetPatientByIdAsync(string patientId);
        public Task<IEnumerable<PatientProfile>> GetPatientsAsync();
        Task<bool> EditPatientBasicInfoAsync(EditPatientBasicInfoViewModel patientProfile);

        Task<bool> EditPatientProfilePictureAsync(PatientProfilePictureViewModel patientProfile);
       
        Task<bool> EditPatientAddressAsync(PatientAddressViewModel patientProfile);
        Task<bool> EditPatientHealthAsync(PatientHealthViewModel patientProfile);
      //  Task<dynamic> GetPatientAppointmentByIdAsync(string patientId);

    }
}
