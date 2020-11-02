using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class PharmacyProfileRepository : IPharmacyProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PharmacyProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public async Task<object> GetPharmacyProfileByIdAsync(string Id)
        {
            var pharmacyProfile = _applicationDbContext.ApplicationUsers.Where(p => p.Id == Id)
                       .Join(
                           _applicationDbContext.PharmacyProfiles,
                           applicationUser => applicationUser.Id,
                           pharmacyProfile => pharmacyProfile.PharmacyId,
                           (applicationUser, pharmacyProfile) => new { applicationUser, pharmacyProfile }
                       ).FirstAsync();

            return await pharmacyProfile;
          
        }
        public async Task<object> GetAllPharmacyAsync() {

            var pharmacistProfiles = _applicationDbContext.ApplicationUsers.Where(a => a.UserType == "pharmacy")
                      .Join(
                           _applicationDbContext.PharmacyProfiles,
                           applicationUser => applicationUser.Id,
                           pharmacyProfile => pharmacyProfile.PharmacyId,
                           (applicationUser, pharmacyProfile) => new { applicationUser, pharmacyProfile }
                       ).ToListAsync();

            return await pharmacistProfiles;

            //_applicationDbContext.DoctorProfiles.Include(d => d.DoctorSpecialization);
        }
        public async Task<bool> EditPharmacyProfileAsync(EditPharmacyProfileViewModel editPharmacyProfile)
        {
            //check if this guy has a profile already
            var pharmacy = await _applicationDbContext.PharmacyProfiles.FirstOrDefaultAsync(d => d.Id == editPharmacyProfile.PharmacyId);
            
            // Validate doctor is not null---has no a profile yet
            if (pharmacy == null)
            {
                var profile = new PharmacyProfile()
                {
                    Gender = editPharmacyProfile.Gender,
                    Address = editPharmacyProfile.Address,
                    ZipCode = editPharmacyProfile.ZipCode,
                    City = editPharmacyProfile.City,
                    State = editPharmacyProfile.State,
                    Country = editPharmacyProfile.Country,
                    PharmacyId = editPharmacyProfile.PharmacyId
                };

                _applicationDbContext.PharmacyProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
      
                pharmacy.Gender = editPharmacyProfile.Gender;
                pharmacy.Address = editPharmacyProfile.Address;
                pharmacy.ZipCode = editPharmacyProfile.ZipCode;
                pharmacy.City = editPharmacyProfile.City;
                pharmacy.State = editPharmacyProfile.State;
                pharmacy.Country = editPharmacyProfile.Country;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> EditPharmacyProfilePictureAsync(PharmacyProfilePictureViewModel PharmacyProfile)
        {
            // Retrieve pharmacy by id
            var pharmacy = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == PharmacyProfile.PharmacyId);

            // Validate doctor is not null
            if (pharmacy != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "/wwwroot/ProfilePictures/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(PharmacyProfile.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, PharmacyProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (PharmacyProfile.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, PharmacyProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            PharmacyProfile.ProfilePicture.CopyTo(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        pharmacy.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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

    }
}
