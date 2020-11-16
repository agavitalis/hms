﻿
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
        Task<object> GetDoctorsAsync();
        Task<DoctorProfile> GetDoctorAsync(string DoctorId);
        Task<bool> EditDoctorProfileAsync(EditDoctorProfileViewModel doctorProfile);
        Task<bool> EditDoctorProfilePictureAsync(DoctorProfilePictureViewModel doctorProfile);
        Task<bool> EditDoctorBasicInfoAsync(EditDoctorBasicInfoViewModel doctorProfile);
        Task<bool> EditDoctorContactAsync(DoctorContactViewModel doctorProfile);
        Task<bool> EditDoctorProfessionalDetailsAsync(DoctorProfessionalDetailsViewModel doctorProfile);
        Task<bool> EditDoctorAvaliabilityAsync(DoctorAvaliablityViewModel doctorProfile);
        Task<bool> AddDoctorEducation(IEnumerable<DoctorEducationDtoForCreate>  doctorEductionDto);
        Task<bool> AddDoctorExperience(IEnumerable<DoctorExperienceDtoForCreate> doctorExperienceDto);
        Task<bool> AddDoctorSocial(IEnumerable<DoctorSocialDtoForCreate> doctorSocialDto);
        Task<bool> AddDoctorOfficeTime(IEnumerable<DoctorOfficeTimeDtoForCreate> doctorOfficeTime);
        Task<bool> AddDoctorSkills(IEnumerable<DoctorSkillsDtoForCreate> doctorSkills);
        Task<bool> EditDoctorEduction(string doctorEdurionId, JsonPatchDocument<DoctorEducationDtoForView> doctorEductionDto);
        Task<bool> EditDoctorExperience(string doctorExperienceId, JsonPatchDocument<DoctorExperienceDtoForView> doctorExperienceDto);
        Task<bool> EditDoctorSocials(string doctorSocialId, JsonPatchDocument<DoctorSocialDtoForView> doctorSocialDto);
        Task<bool> EditDoctorOfficeTime(string doctorOfficeTimeId, JsonPatchDocument<DoctorOfficeTimeDtoForView> doctorOfficeTime);
        Task<bool> EditDoctorSkill(string doctorSkilId, JsonPatchDocument<DoctorSkillsDtoForView> doctorOfficeTime);
        Task<bool> DeleteDoctorEduction(string doctorEductionId);
        Task<bool> DeleteDoctorSocial(string doctorSocialId);
        Task<bool> DeleteDoctorExperience(string doctorExperienceId);
        Task<bool> DeleteDoctorOfficeTime(string doctorOfficeTimeId);
        Task<bool> DeleteDoctorSkills(string doctorSkillId);
        Task<IEnumerable<DoctorEducationDtoForView>> GetDoctorEductions(string DoctorId);
        Task<IEnumerable<DoctorExperienceDtoForView>> GetDoctorExperience(string DoctorId);
        Task<IEnumerable<DoctorSocialDtoForView>> GetDoctorSocial(string DoctorId);
        Task<IEnumerable<DoctorOfficeTimeDtoForView>> GetDoctorOfficeTime(string DoctorId);
        Task<IEnumerable<DoctorSkillsDtoForView>> GetDoctorSkills(string DoctorId);
        Task<DoctorEducationDtoForView> GetDoctorEducationById(string doctorEductionId);
        Task<DoctorExperienceDtoForView> GetDoctorExperienceById(string doctorExperienceId);
        Task<DoctorSocialDtoForView> GetDoctorSocialById(string doctorSocialId);
        Task<DoctorOfficeTimeDtoForView> GetDoctorOfficeTimeById(string doctorOfficeTimeId);
        Task<DoctorSkillsDtoForView> GetDoctorSkillById(string doctorSkillId);
        string GetDoctorProfileId(string doctorId);
    }
}
