using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Doctor.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorProfileRepository : IDoctorProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DoctorProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<int> GetDoctorCountAsync() => await _applicationDbContext.DoctorProfiles.CountAsync();

        public async Task<object> GetDoctorsAsync()
        {
            var doctors = await _applicationDbContext.DoctorProfiles
                .Include(d => d.Doctor)
                //.Include(p => p.Experiences)
                //.Include(p => p.Educations)
                //.Include(p => p.OfficeTimes)
                //.Include(p => p.Socials)
                //.Include(p => p.Skills)
                .ToListAsync();

            return doctors;
        }

        public async Task<DoctorProfile> GetDoctorAsync(string DoctorId)
        {
            var doctors = await _applicationDbContext.DoctorProfiles.Where(p => p.DoctorId == DoctorId)
                .Include(p => p.Doctor)
                 .Include(p => p.Experiences)
                .Include(p => p.Educations)
                .Include(p => p.OfficeTime)
                .Include(p => p.Socials)
                .Include(p => p.Skills)
                .FirstOrDefaultAsync();

          
            return doctors;
        }

        public string GetDoctorProfileId(string doctorId)
        {
            return _applicationDbContext.DoctorProfiles.Where(d => d.DoctorId == doctorId).FirstOrDefault().Id;
        }

        public async Task<bool> EditDoctorProfileAsync(EditDoctorProfileViewModel editDoctorProfile)
        {
            //check if this guy has a profile already
            var Doctor = await _applicationDbContext.DoctorProfiles.FirstOrDefaultAsync(d => d.DoctorId == editDoctorProfile.DoctorId);

            // Validate doctor is not null---has no a profile yet
            if (Doctor == null)
            {
                var profile = new DoctorProfile()
                {
                    Age = editDoctorProfile.Age,
                    Gender = editDoctorProfile.Gender,
                    Address = editDoctorProfile.Address,
                    DoctorId = editDoctorProfile.DoctorId
                };

                _applicationDbContext.DoctorProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                Doctor.Age = editDoctorProfile.Age;
                Doctor.Gender = editDoctorProfile.Gender;
                Doctor.Address = editDoctorProfile.Address;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditDoctorBasicInfoAsync(EditDoctorBasicInfoViewModel doctorProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var Doctor = await _applicationDbContext.DoctorProfiles.FirstOrDefaultAsync(d => d.DoctorId == doctorProfile.DoctorId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == doctorProfile.DoctorId);

            // Validate patient is not null---has no profile yet
            if (Doctor == null)
            {
                var profile = new DoctorProfile()
                {
                    Age = doctorProfile.Age,
                    Gender = doctorProfile.Gender,
                    DateOfBirth = doctorProfile.DateOfBirth,
                    DoctorId = doctorProfile.DoctorId

                };
                User.FirstName = doctorProfile.FirstName;
                User.LastName = doctorProfile.LastName;
                User.OtherNames = doctorProfile.OtherNames;

                _applicationDbContext.DoctorProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                User.FirstName = doctorProfile.FirstName;
                User.LastName = doctorProfile.LastName;
                User.OtherNames = doctorProfile.OtherNames;
                Doctor.Age = doctorProfile.Age;

                Doctor.Gender = doctorProfile.Gender;
                Doctor.DateOfBirth = doctorProfile.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> EditDoctorProfilePictureAsync(DoctorProfilePictureViewModel doctorProfile)
        {
            // Retrieve Doctor by id
            var Doctor = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == doctorProfile.DoctorId);

            // Validate doctor is not null
            if (Doctor != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "wwwroot/Images/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(doctorProfile.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, doctorProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (doctorProfile.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, doctorProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            await doctorProfile.ProfilePicture.CopyToAsync(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        Doctor.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

                        // Save changes in database
                        await _applicationDbContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EditDoctorContactAsync(DoctorContactViewModel doctorProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var Doctor = await _applicationDbContext.DoctorProfiles.FirstOrDefaultAsync(d => d.DoctorId == doctorProfile.DoctorId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == doctorProfile.DoctorId);
            // Validate patient is not null---has no profile yet
            if (Doctor == null)
            {
                var profile = new DoctorProfile()
                {
                    Address = doctorProfile.Address,
                    ZipCode = doctorProfile.ZipCode,
                    Country = doctorProfile.Country,
                    State = doctorProfile.State,
                    City = doctorProfile.City,
                    DoctorId = doctorProfile.DoctorId

                };

                User.PhoneNumber = doctorProfile.PhoneNumber;
                User.Email = doctorProfile.Email;
                _applicationDbContext.DoctorProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                Doctor.Address = doctorProfile.Address;
                Doctor.ZipCode = doctorProfile.ZipCode;
                Doctor.Country = doctorProfile.Country;
                Doctor.State = doctorProfile.State;
                Doctor.City = doctorProfile.City;
                User.PhoneNumber = doctorProfile.PhoneNumber;
                User.Email = doctorProfile.Email;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> EditDoctorProfessionalDetailsAsync(DoctorProfessionalDetailsViewModel doctorProfile)
        {
            //check if this guy has a profile already
            var Doctor = await _applicationDbContext.DoctorProfiles.FirstOrDefaultAsync(d => d.DoctorId == doctorProfile.DoctorId);
            // Validate patient is not null---has no profile yet
            if (Doctor == null)
            {
                var profile = new DoctorProfile()
                {
                    //About = doctorProfile.About,
                    //Education = doctorProfile.Education,
                    //Specialization = doctorProfile.Specialization,
                    DoctorId = doctorProfile.DoctorId

                };
                _applicationDbContext.DoctorProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                //Doctor.About = doctorProfile.About;
                //Doctor.Education = doctorProfile.Education;
                //Doctor.Specialization = doctorProfile.Specialization;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> EditDoctorAvaliabilityAsync(DoctorAvaliablityViewModel doctorProfile)
        {
            //check if this guy has a profile already
            var Doctor = await _applicationDbContext.DoctorProfiles.FirstOrDefaultAsync(d => d.DoctorId == doctorProfile.DoctorId);
            // Validate patient is not null---has no profile yet
            if (Doctor == null)
            {
                var profile = new DoctorProfile()
                {
                    //OfficeHours = doctorProfile.OfficeHours,
                    //IsAvaliable = doctorProfile.IsAvaliable,

                    DoctorId = doctorProfile.DoctorId

                };
                _applicationDbContext.DoctorProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                //Doctor.OfficeHours = doctorProfile.OfficeHours;
                //Doctor.IsAvaliable = doctorProfile.IsAvaliable;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> AddDoctorEducation(IEnumerable<DoctorEducationDtoForCreate> doctorEductionDto)
        {
            try
            {
                if (!doctorEductionDto.Any())
                    return false;

                var doctorEducation = _mapper.Map<IEnumerable<DoctorEducation>>(doctorEductionDto);

                _applicationDbContext.DoctorEducations.AddRange(doctorEducation);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddDoctorExperience(IEnumerable<DoctorExperienceDtoForCreate> doctorExperienceDto)
        {
            try
            {
                if (!doctorExperienceDto.Any())
                    return false;

                var doctorExperiences = _mapper.Map<IEnumerable<DoctorExperience>>(doctorExperienceDto);

                _applicationDbContext.DoctorExperiences.AddRange(doctorExperiences);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddDoctorSocial(IEnumerable<DoctorSocialDtoForCreate> doctorSocialDto)
        {
            try
            {
                if (!doctorSocialDto.Any())
                    return false;

                var doctorSocials = _mapper.Map<IEnumerable<DoctorSocial>>(doctorSocialDto);

                _applicationDbContext.DoctorSocials.AddRange(doctorSocials);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddDoctorOfficeTime(IEnumerable<DoctorOfficeTimeDtoForCreate> doctorOfficeTimeDto)
        {
            try
            {
                if (!doctorOfficeTimeDto.Any())
                    return false;

                var doctorOfficeTimes = _mapper.Map<IEnumerable<DoctorOfficeTime>>(doctorOfficeTimeDto);

                _applicationDbContext.DoctorOfficeTimes.AddRange(doctorOfficeTimes);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditDoctorEduction(string doctorEdurionId, JsonPatchDocument<DoctorEducationDtoForView> doctorEductionDto)
        {
            try
            {
                var doctorEducation = _applicationDbContext.DoctorEducations.Find(doctorEdurionId);

                if (doctorEducation != null)
                {
                    var doctorEducationToMap = _mapper.Map<DoctorEducationDtoForView>(doctorEducation);
                    doctorEductionDto.ApplyTo(doctorEducationToMap);

                    var doctorExperienceToUpdate = _mapper.Map<DoctorEducation>(doctorEducationToMap);

                    _applicationDbContext.DoctorEducations.Update(doctorExperienceToUpdate);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> EditDoctorExperience(string experienceId, JsonPatchDocument<DoctorExperienceDtoForView> doctorExperienceDto)
        {
            try
            {
                var doctorExperience = _applicationDbContext.DoctorExperiences.Find(experienceId);

                if (doctorExperience != null)
                {
                    var doctorExperienceToMap = _mapper.Map<DoctorExperienceDtoForView>(doctorExperience);
                    doctorExperienceDto.ApplyTo(doctorExperienceToMap);

                    var doctorExperienceToUpdate = _mapper.Map<DoctorExperience>(doctorExperienceToMap);

                    _applicationDbContext.DoctorExperiences.Update(doctorExperienceToUpdate);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> EditDoctorSocials(string socialId, JsonPatchDocument<DoctorSocialDtoForView> doctorSocialDto)
        {
            try
            {
                var doctorSocial = _applicationDbContext.DoctorSocials.Find(socialId);

                if (doctorSocial != null)
                {
                    var doctorSocialToMap = _mapper.Map<DoctorSocialDtoForView>(doctorSocial);
                    doctorSocialDto.ApplyTo(doctorSocialToMap);

                    var doctorSocialToUpdate = _mapper.Map<DoctorSocial>(doctorSocialToMap);

                    _applicationDbContext.DoctorSocials.Update(doctorSocialToUpdate);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> EditDoctorOfficeTime(string officeTimeId, JsonPatchDocument<DoctorOfficeTimeDtoForView> doctorOfficeTime)
        {
            try
            {
                var OfficeTime = _applicationDbContext.DoctorOfficeTimes.Find(officeTimeId);

                if (OfficeTime != null)
                {
                    var doctorOfficeTimeToUpdate = _mapper.Map<DoctorOfficeTimeDtoForView>(OfficeTime);
                    doctorOfficeTime.ApplyTo(doctorOfficeTimeToUpdate);

                    var doctorOfficeToUpdate = _mapper.Map<DoctorOfficeTime>(doctorOfficeTimeToUpdate);

                    _applicationDbContext.DoctorOfficeTimes.Update(doctorOfficeToUpdate);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> DeleteDoctorEduction(string id)
        {
            try
            {
                var education = _applicationDbContext.DoctorEducations.Find(id);

                if (education != null)
                {
                    _applicationDbContext.DoctorEducations.Remove(education);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> DeleteDoctorSocial(string id)
        {
            try
            {
                var doctorSocial = _applicationDbContext.DoctorSocials.Find(id);

                if (doctorSocial != null)
                {
                    _applicationDbContext.DoctorSocials.Remove(doctorSocial);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> DeleteDoctorExperience(string id)
        {
            try
            {
                var doctorExperience = _applicationDbContext.DoctorExperiences.Find(id);

                if (doctorExperience != null)
                {
                    _applicationDbContext.DoctorExperiences.Remove(doctorExperience);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> DeleteDoctorOfficeTime(string id)
        {
            try
            {

                var officeTime = _applicationDbContext.DoctorOfficeTimes.Find(id);

                if (officeTime != null)
                {
                    _applicationDbContext.DoctorOfficeTimes.Remove(officeTime);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<DoctorEducationDtoForView>> GetDoctorEductions(string doctorId)
        {
            var educations = await _applicationDbContext.DoctorEducations.Where(x => x.DoctorId == doctorId).ToListAsync();

            var doctorEducations = _mapper.Map<IEnumerable<DoctorEducationDtoForView>>(educations);

            return doctorEducations;
        }

        public async Task<IEnumerable<DoctorExperienceDtoForView>> GetDoctorExperience(string doctorId)
        {
            var experiences = await _applicationDbContext.DoctorExperiences.Where(x => x.DoctorId == doctorId).ToListAsync();

            var doctorExperience = _mapper.Map<IEnumerable<DoctorExperienceDtoForView>>(experiences);

            return doctorExperience;
        }

        public async Task<IEnumerable<DoctorSocialDtoForView>> GetDoctorSocial(string doctorId)
        {
            var socials = await _applicationDbContext.DoctorSocials.Where(x => x.DoctorId == doctorId).ToListAsync();

            var socialsMapped = _mapper.Map<IEnumerable<DoctorSocialDtoForView>>(socials);

            return socialsMapped;
        }

        public async Task<IEnumerable<DoctorOfficeTimeDtoForView>> GetDoctorOfficeTime(string doctorId)
        {
            var officeTimes = await _applicationDbContext.DoctorOfficeTimes.Where(x => x.DoctorId == doctorId).ToListAsync();

            var officeTimeMapped = _mapper.Map<IEnumerable<DoctorOfficeTimeDtoForView>>(officeTimes);

            return officeTimeMapped;
        }

        public async Task<DoctorEducationDtoForView> GetDoctorEducationById(string doctorEductionId)
        {
            var doctorEducation = await _applicationDbContext.DoctorEducations.FindAsync(doctorEductionId);

            return _mapper.Map<DoctorEducationDtoForView>(doctorEducation);
        }

        public async Task<DoctorExperienceDtoForView> GetDoctorExperienceById(string doctorExperienceId)
        {
            var doctorExperience = await _applicationDbContext.DoctorExperiences.FindAsync(doctorExperienceId);

            return _mapper.Map<DoctorExperienceDtoForView>(doctorExperience);
        }

        public async Task<DoctorSocialDtoForView> GetDoctorSocialById(string doctorSocialId)
        {
            var doctorSocial = await _applicationDbContext.DoctorSocials.FindAsync(doctorSocialId);

            return _mapper.Map<DoctorSocialDtoForView>(doctorSocial);
        }

        public async Task<DoctorOfficeTimeDtoForView> GetDoctorOfficeTimeById(string doctorOfficeTimeId)
        {
            var doctorOfficeTime = await _applicationDbContext.DoctorOfficeTimes.FindAsync(doctorOfficeTimeId);

            return _mapper.Map<DoctorOfficeTimeDtoForView>(doctorOfficeTime);
        }

        public async Task<bool> AddDoctorSkills(IEnumerable<DoctorSkillsDtoForCreate> doctorSkills)
        {
            try
            {
                if (!doctorSkills.Any())
                    return false;

                var doctorSkillsToAdd = _mapper.Map<IEnumerable<DoctorSkills>>(doctorSkills);

                _applicationDbContext.DoctorSkills.AddRange(doctorSkillsToAdd);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditDoctorSkill(string doctorSkilId, JsonPatchDocument<DoctorSkillsDtoForView> doctorSkillPatch)
        {
            try
            {
                var skill = _applicationDbContext.DoctorSkills.Find(doctorSkilId);

                if (skill != null)
                {
                    var doctorSkillToUpdate = _mapper.Map<DoctorSkillsDtoForView>(skill);
                    doctorSkillPatch.ApplyTo(doctorSkillToUpdate);

                    var doctorOfficeToUpdate = _mapper.Map<DoctorOfficeTime>(doctorSkillToUpdate);

                    _applicationDbContext.DoctorOfficeTimes.Update(doctorOfficeToUpdate);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> DeleteDoctorSkills(string doctorSkillId)
        {
            try
            {
                var doctorSkill = _applicationDbContext.DoctorSkills.Find(doctorSkillId);

                if (doctorSkill != null)
                {
                    _applicationDbContext.DoctorSkills.Remove(doctorSkill);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<DoctorSkillsDtoForView>> GetDoctorSkills(string doctorId)
        {
            var skills = await _applicationDbContext.DoctorSkills.Where(x => x.DoctorId == doctorId).ToListAsync();

            var skillsMapped = _mapper.Map<IEnumerable<DoctorSkillsDtoForView>>(skills);

            return skillsMapped;
        }

        public async Task<DoctorSkillsDtoForView> GetDoctorSkillById(string doctorSkillId)
        {
            var doctorOfficeTime = await _applicationDbContext.DoctorSkills.FindAsync(doctorSkillId);

            return _mapper.Map<DoctorSkillsDtoForView>(doctorOfficeTime);
        }
    }
}
