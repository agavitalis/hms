using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
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

namespace HMS.Areas.Lab.Repositories
{
    public class LabProfileRepository : ILabProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public LabProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public async Task<object> GetLabByIdAsync(string Id)
        {
            var LabProfile = _applicationDbContext.ApplicationUsers.Where(p => p.Id == Id)
                       .Join(
                           _applicationDbContext.LabProfiles,
                           applicationUser => applicationUser.Id,
                           LabProfile => LabProfile.LabId,
                           (applicationUser, LabProfile) => new { applicationUser, LabProfile }
                       ).FirstAsync();

            return await LabProfile;

        }
        public async Task<object> GetAllLabAsync()
        {

            var labProfiles = _applicationDbContext.ApplicationUsers.Where(a => a.UserType == "Lab")
                      .Join(
                           _applicationDbContext.LabProfiles,
                           applicationUser => applicationUser.Id,
                           LabProfile => LabProfile.LabId,
                           (applicationUser, LabProfile) => new { applicationUser, LabProfile }
                       ).ToListAsync();

            return await labProfiles;

            //_applicationDbContext.DoctorProfiles.Include(d => d.DoctorSpecialization);
        }
        public async Task<bool> EditLabProfileAsync(EditLabProfileViewModel editLabProfile)
        {
            //check if this guy has a profile already
            var Lab = await _applicationDbContext.LabProfiles.FirstOrDefaultAsync(d => d.Id == editLabProfile.LabId);

            // Validate doctor is not null---has no a profile yet
            if (Lab == null)
            {
                var profile = new LabProfile()
                {
                    Gender = editLabProfile.Gender,
                    Address = editLabProfile.Address,
                    ZipCode = editLabProfile.ZipCode,
                    City = editLabProfile.City,
                    State = editLabProfile.State,
                    Country = editLabProfile.Country,
                    LabId = editLabProfile.LabId
                };

                _applicationDbContext.LabProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                Lab.Gender = editLabProfile.Gender;
                Lab.Address = editLabProfile.Address;
                Lab.ZipCode = editLabProfile.ZipCode;
                Lab.City = editLabProfile.City;
                Lab.State = editLabProfile.State;
                Lab.Country = editLabProfile.Country;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditLabProfilePictureAsync(LabProfilePictureViewModel LabProfile)
        {
            // Retrieve Lab by id
            var Lab = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == LabProfile.LabId);

            // Validate doctor is not null
            if (Lab != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "/wwwroot/ProfilePictures/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(LabProfile.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, LabProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (LabProfile.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, LabProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            LabProfile.ProfilePicture.CopyTo(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        Lab.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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

        //public async Task<bool> DeleteLabProfileAsync(string LabId)
        //{

        //    // Retrieve lab by id
        //    var Lab = await _applicationDbContext.LabProfiles.FirstOrDefaultAsync(d => d.LabId == LabId);
        //    var _Lab = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == LabId);

        //    // Validate Lab selected is not null
        //    if (_Lab != null)
        //    {
        //        //Delete Lab From Database
        //        _applicationDbContext.LabProfiles.Remove(Lab);
        //        _applicationDbContext.ApplicationUsers.Remove(_Lab);

        //        // Save changes in database
        //        await _applicationDbContext.SaveChangesAsync();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
