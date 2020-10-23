using HMS.Database;
using HMS.Services.Helpers;
using HMS.Services.Interfaces.Doctor;
using HMS.ViewModels.Doctor;
using HMS.Models.Doctor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Collections;
using HMS.Models;
using static HMS.ViewModels.Doctor.DoctorSpecializationViewModel;

namespace HMS.Services.Repositories.Doctor
{
    public class DoctorProfileRepository : IDoctorProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DoctorProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<object> GetDoctorByIdAsync(string DoctorId)
        {
            var doctor = _applicationDbContext.ApplicationUsers.Where(p => p.Id == DoctorId).FirstAsync();
            return await doctor;
        }

        public async Task<object> GetDoctorProfileByIdAsync(string DoctorId)
        {
            var doctorProfile = _applicationDbContext.ApplicationUsers.Where(p => p.Id == DoctorId)
                       .Join(
                           _applicationDbContext.DoctorProfiles,
                           applicationUser => applicationUser.Id,
                           doctorProfile => doctorProfile.DoctorId,        
                           (applicationUser, doctorProfile) => new { applicationUser, doctorProfile }
                       )
                       .ToListAsync();

            return await doctorProfile;

        }

        public async Task<object> GetDoctorSpecializationByIdAsync(string DoctorId)
        {
            var doctorSpecialization = _applicationDbContext.ApplicationUsers.Where(p => p.Id == DoctorId)
                        .Join(
                            _applicationDbContext.DoctorSpecializations,
                            applicationUser => applicationUser.Id,
                            doctorSpecialization => doctorSpecialization.DoctorId,
                            (applicationUser, doctorSpecialization) => new { applicationUser, doctorSpecialization }
                       )

                       .ToListAsync();

            return await doctorSpecialization;

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
                var folderToSaveIn = "/wwwroot/ProfilePictures/";
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
                            doctorProfile.ProfilePicture.CopyTo(fileStream);
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
                    About = doctorProfile.About,
                    Education = doctorProfile.Education,
                    Specialization = doctorProfile.Specialization,
                    DoctorId = doctorProfile.DoctorId

                };
                _applicationDbContext.DoctorProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                Doctor.About = doctorProfile.About;
                Doctor.Education = doctorProfile.Education;
                Doctor.Specialization = doctorProfile.Specialization;
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
                    OfficeHours = doctorProfile.OfficeHours,
                    IsAvaliable = doctorProfile.IsAvaliable,
              
                    DoctorId = doctorProfile.DoctorId

                };
                _applicationDbContext.DoctorProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                Doctor.OfficeHours = doctorProfile.OfficeHours;
                Doctor.IsAvaliable = doctorProfile.IsAvaliable;
      
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }





    }
}
