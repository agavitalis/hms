
using HMS.ViewModels.Doctor;
using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Doctor
{
    public interface IDoctorProfile
    {
        Task<object> GetDoctorByIdAsync(string DoctorId);
        Task<object> GetDoctorProfileByIdAsync(string DoctorId);
        Task<object> GetDoctorSpecializationByIdAsync(string DoctorId);
        Task<bool> EditDoctorProfileAsync(EditDoctorProfileViewModel doctorProfile);
        Task<bool> EditDoctorProfilePictureAsync(DoctorProfilePictureViewModel doctorProfile);
        Task<bool> EditDoctorBasicInfoAsync(EditDoctorBasicInfoViewModel doctorProfile);
        Task<bool> EditDoctorContactAsync(DoctorContactViewModel doctorProfile);
        Task<bool> EditDoctorProfessionalDetailsAsync(DoctorProfessionalDetailsViewModel doctorProfile);
        Task<bool> EditDoctorAvaliabilityAsync(DoctorAvaliablityViewModel doctorProfile);

    }
}
