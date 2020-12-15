using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class AdminProfileRepository : IAdminProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public AdminProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public async Task<object> GetAdmin(string AdminId) => await _applicationDbContext.AdminProfiles.Where(a => a.AdminId == AdminId).Include(a => a.Admin).FirstOrDefaultAsync();

        public async Task<object> GetAdmins() => await _applicationDbContext.AdminProfiles.Include(a => a.Admin).ToListAsync();

        public async Task<bool> EditAdminBasicInfo(EditAdminBasicInfoViewModel AdminProfile)
        {
            //check if this guy has a profile already
            var adminProfile = await _applicationDbContext.AdminProfiles.FirstOrDefaultAsync(a => a.AdminId == AdminProfile.AdminId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == adminProfile.AdminId);
            // Validate account profile is not null---has no a profile yet
            if (adminProfile == null)
            {
                var profile = new AdminProfile()
                {
                    FullName = AdminProfile.FirstName + " " + AdminProfile.LastName,
                    Age = AdminProfile.Age,
                    DateOfBirth = AdminProfile.DateOfBirth,
                    Gender = AdminProfile.Gender,
                    AdminId = AdminProfile.AdminId
                };

                User.FirstName = AdminProfile.FirstName;
                User.LastName = AdminProfile.LastName;
                User.OtherNames = AdminProfile.OtherNames;

                _applicationDbContext.AdminProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = AdminProfile.FirstName;
                User.LastName = AdminProfile.LastName;
                User.OtherNames = AdminProfile.OtherNames;
                adminProfile.FullName = AdminProfile.FirstName + " " + AdminProfile.LastName;
                adminProfile.Age = AdminProfile.Age;
                adminProfile.Gender = AdminProfile.Gender;
                adminProfile.DateOfBirth = AdminProfile.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditAdminContactDetails(EditAdminContactDetailsViewModel Admin)
        {
            //check if this guy has a profile already
            var admin = await _applicationDbContext.AdminProfiles.FirstOrDefaultAsync(d => d.AdminId == Admin.AdminId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Admin.AdminId);
            // Validate patient is not null---has no profile yet
            if (admin == null)
            {
                var profile = new AdminProfile()
                {
                    Address = Admin.Address,
                    ZipCode = Admin.ZipCode,
                    Country = Admin.Country,
                    State = Admin.State,
                    City = Admin.City,
                    AdminId = Admin.AdminId
                };

                User.PhoneNumber = Admin.PhoneNumber;
                User.Email = Admin.Email;
                _applicationDbContext.AdminProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                admin.Address = Admin.Address;
                admin.ZipCode = Admin.ZipCode;
                admin.Country = Admin.Country;
                admin.State = Admin.State;
                admin.City = Admin.City;
                User.PhoneNumber = Admin.PhoneNumber;
                User.Email = Admin.Email;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditAdminProfilePictureAsync(AdminProfilePictureViewModel AdminProfile)
        {
            // Retrieve Accountant by id
            var adminProfile = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == AdminProfile.AdminProfileId);

            // Validate doctor is not null
            if (adminProfile != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "/wwwroot/ProfilePictures/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(AdminProfile.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, AdminProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (AdminProfile.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, AdminProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            AdminProfile.ProfilePicture.CopyTo(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        adminProfile.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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