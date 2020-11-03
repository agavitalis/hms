
using HMS.Areas.Doctor.ViewModels;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorProfile
    {
        Task<object> GetDoctorsAsync();
        Task<object> GetDoctorAsync(string DoctorId);
        Task<bool> EditDoctorProfileAsync(EditDoctorProfileViewModel doctorProfile);
        Task<bool> EditDoctorProfilePictureAsync(DoctorProfilePictureViewModel doctorProfile);
        Task<bool> EditDoctorBasicInfoAsync(EditDoctorBasicInfoViewModel doctorProfile);
        Task<bool> EditDoctorContactAsync(DoctorContactViewModel doctorProfile);
        Task<bool> EditDoctorProfessionalDetailsAsync(DoctorProfessionalDetailsViewModel doctorProfile);
        Task<bool> EditDoctorAvaliabilityAsync(DoctorAvaliablityViewModel doctorProfile);

    }
}
