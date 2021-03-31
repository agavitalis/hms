
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorProfile
    {
        Task<int> GetDoctorCountAsync();
        
        PagedList<DoctorDtoForView> GetDoctors(PaginationParameter paginationParameter);
        Task<DoctorDtoForView> GetDoctor(string DoctorId);
        
        Task<object> GetDoctorsByPatient(string PatientId);
        Task<object> GetDoctorsBySpecialization(string doctorSpecialiazation);
        Task<DoctorProfile> GetDoctorAsync(string DoctorId);
        Task<bool> EditDoctorProfileAsync(EditDoctorProfileViewModel doctorProfile);
        Task<bool> EditDoctorProfilePictureAsync(DoctorProfilePictureViewModel doctorProfile);
        Task<bool> EditDoctorBasicInfoAsync(EditDoctorBasicInfoViewModel doctorProfile);
        Task<bool> EditDoctorContactAsync(DoctorContactViewModel doctorProfile);
        Task<bool> EditDoctorProfessionalDetailsAsync(DoctorProfessionalDetailsViewModel doctorProfile);
        Task<bool> EditDoctorAvaliabilityAsync(string DoctorId);
        Task<bool> AddDoctorEducation(IEnumerable<DoctorEducationDtoForCreate>  doctorEductionDto);
        Task<bool> AddDoctorExperience(IEnumerable<DoctorExperienceDtoForCreate> doctorExperienceDto);
        Task<bool> AddDoctorSocial(IEnumerable<DoctorSocialDtoForCreate> doctorSocialDto);
        Task<bool> AddDoctorOfficeTime(IEnumerable<DoctorOfficeTimeDtoForCreate> doctorOfficeTime);
        Task<bool> AddDoctorSpecializations(IEnumerable<DoctorSpecializationsDtoForCreate> doctorSpecializations);
        Task<bool> EditDoctorEduction(string doctorEdurionId, JsonPatchDocument<DoctorEducationDtoForView> doctorEductionDto);
        Task<bool> EditDoctorExperience(string doctorExperienceId, JsonPatchDocument<DoctorExperienceDtoForView> doctorExperienceDto);
        Task<bool> EditDoctorSocials(string doctorSocialId, JsonPatchDocument<DoctorSocialDtoForView> doctorSocialDto);
        Task<bool> EditDoctorOfficeTime(string doctorOfficeTimeId, JsonPatchDocument<DoctorOfficeTimeDtoForView> doctorOfficeTime);
        Task<bool> EditDoctorSpecialization(string doctorSkilId, JsonPatchDocument<DoctorSpecializationsDtoForView> doctorOfficeTime);
        Task<bool> DeleteDoctorEduction(string doctorEductionId);
        Task<bool> DeleteDoctorSocial(string doctorSocialId);
        Task<bool> DeleteDoctorExperience(string doctorExperienceId);
        Task<bool> DeleteDoctorOfficeTime(string doctorOfficeTimeId);
        Task<bool> DeleteDoctorSpecializations(string doctorSpecializationId);
        Task<IEnumerable<DoctorEducationDtoForView>> GetDoctorEductions(string DoctorId);
        Task<IEnumerable<DoctorExperienceDtoForView>> GetDoctorExperience(string DoctorId);
        Task<IEnumerable<DoctorSocialDtoForView>> GetDoctorSocial(string DoctorId);
        Task<IEnumerable<DoctorOfficeTimeDtoForView>> GetDoctorOfficeTime(string DoctorId);
        Task<IEnumerable<DoctorSpecializationsDtoForView>> GetDoctorSpecializations(string DoctorId);
        Task<DoctorEducationDtoForView> GetDoctorEducationById(string doctorEductionId);
        Task<DoctorExperienceDtoForView> GetDoctorExperienceById(string doctorExperienceId);
        Task<DoctorSocialDtoForView> GetDoctorSocialById(string doctorSocialId);
        Task<DoctorOfficeTimeDtoForView> GetDoctorOfficeTimeById(string doctorOfficeTimeId);
        Task<DoctorSpecializationsDtoForView> GetDoctorSpecializationById(string doctorSpecializationId);
        string GetDoctorProfileId(string doctorId);
    }
}
