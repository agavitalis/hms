using HMS.Models;
using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Doctor.DoctorSpecializationViewModel;

namespace HMS.Services.Interfaces.Doctor
{
    public interface IDoctorSpecialization
    {
        Task<object> GetDoctorSpecializationAsync(string DoctorId);
        Task<bool> CreateDoctorSpecializationAsync(CreateDoctorSpecializationViewModel createDoctorSpecialization);
        Task<bool> EditDoctorSpecializationAsync(EditDoctorSpecializationViewModel editDoctorSpecialization);
        Task<bool> DeleteDoctorSpecializationAsync(string SpecializationId);
    }
}
