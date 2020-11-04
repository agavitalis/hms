using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Accountant.ViewModels;
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

namespace HMS.Areas.Accountant.Repositories
{
    public class AccountantProfileRepository : IAccountantProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public AccountantProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

       

     

        public async Task<object> GetAccountantByIdAsync(string AccountantId)
        {
            //check if this guy has a profile already
            var accountProfile = await _applicationDbContext.AccountantProfiles.FirstOrDefaultAsync(a => a.AccountantId == AccountantId);
            if (accountProfile == null)
            {
                return null;
            }
            
            var AccountProfile = _applicationDbContext.ApplicationUsers.Where(p => p.Id == AccountantId)
                      .Join(
                          _applicationDbContext.AccountantProfiles,
                          applicationUser => applicationUser.Id,
                          AccountProfile => AccountProfile.AccountantId,
                          (applicationUser, AccountProfile) => new { applicationUser, AccountProfile }
                      )

                     .FirstAsync();
            return await AccountProfile;
            
            //if (AccountProfile.Status == WaitingForActivation)
            //{
            //    return null;
            //}
            

        }


        public async Task<bool> EditAccountProfileAsync(EditAccountProfileViewModel editAccountProfile)
        {
            //check if this guy has a profile already
            var accountProfile = await _applicationDbContext.AccountantProfiles.FirstOrDefaultAsync(a => a.AccountantId == editAccountProfile.AccountantId);

            // Validate account profile is not null---has no a profile yet
            if (accountProfile == null)
            {
                var profile = new AccountantProfile()
                {
                    Age = editAccountProfile.Age,
                    Gender = editAccountProfile.Gender,
                    BloodGroup = editAccountProfile.BloodGroup,
                    GenoType = editAccountProfile.GenoType,
                    Address = editAccountProfile.Address, 
                    AccountantId = editAccountProfile.AccountantId
                };

                _applicationDbContext.AccountantProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                accountProfile.Age = editAccountProfile.Age;
                accountProfile.Gender = editAccountProfile.Gender;
                accountProfile.Address = editAccountProfile.Address;
                accountProfile.BloodGroup = editAccountProfile.BloodGroup;
                accountProfile.GenoType = editAccountProfile.GenoType;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditAccountProfilePictureAsync(AccountProfilePictureViewModel AccountProfile)
        {
            // Retrieve Accountant by id
            var accountProfile = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == AccountProfile.AccountProfileId);

            // Validate doctor is not null
            if (accountProfile != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "/wwwroot/ProfilePictures/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(AccountProfile.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, AccountProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (AccountProfile.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, AccountProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            AccountProfile.ProfilePicture.CopyTo(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        accountProfile.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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
