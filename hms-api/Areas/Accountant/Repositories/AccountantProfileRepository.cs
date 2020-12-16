using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Accountant.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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





        public async Task<object> GetAccountant(string AccountantId) => await _applicationDbContext.AccountantProfiles.Where(a => a.AccountantId == AccountantId).Include(a => a.Accountant).FirstOrDefaultAsync();

        public async Task<object> GetAccountants() => await _applicationDbContext.AccountantProfiles.Include(a => a.Accountant).ToListAsync();


        public async Task<bool> EditAccountantBasicInfo(EditAccountantBasicInfoViewModel AccountProfile)
        {
            //check if this guy has a profile already
            var accountProfile = await _applicationDbContext.AccountantProfiles.FirstOrDefaultAsync(a => a.AccountantId == AccountProfile.AccountantId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == accountProfile.AccountantId);
            // Validate account profile is not null---has no a profile yet
            if (accountProfile == null)
            {
                var profile = new AccountantProfile()
                {
                  FullName = AccountProfile.FirstName +" "+ AccountProfile.LastName,
                  
                  Age = AccountProfile.Age,
                  DateOfBirth = AccountProfile.DateOfBirth,
                  Gender = AccountProfile.Gender,
                  AccountantId = AccountProfile.AccountantId
                };

                User.FirstName = AccountProfile.FirstName;
                User.LastName = AccountProfile.LastName;
                User.OtherNames = AccountProfile.OtherNames;
                
                _applicationDbContext.AccountantProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = AccountProfile.FirstName;
                User.LastName = AccountProfile.LastName;
                User.OtherNames = AccountProfile.OtherNames;
                accountProfile.FullName = AccountProfile.FirstName + " " + AccountProfile.LastName;
                accountProfile.Age = AccountProfile.Age;
                accountProfile.Gender = AccountProfile.Gender;
                accountProfile.DateOfBirth = AccountProfile.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditAccountantContactDetails(EditAccountantContactDetailsViewModel Accountant)
        {
            //check if this guy has a profile already
            var accountant = await _applicationDbContext.AccountantProfiles.FirstOrDefaultAsync(d => d.AccountantId == Accountant.AccountantId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Accountant.AccountantId);
            // Validate patient is not null---has no profile yet
            if (accountant == null)
            {
                var profile = new AccountantProfile()
                {
                    Address = Accountant.Address,
                    ZipCode = Accountant.ZipCode,
                    Country = Accountant.Country,
                    State = Accountant.State,
                    City = Accountant.City,
                    AccountantId = Accountant.AccountantId
                };

                User.PhoneNumber = Accountant.PhoneNumber;
                User.Email = Accountant.Email;
                _applicationDbContext.AccountantProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                accountant.Address = Accountant.Address;
                accountant.ZipCode = Accountant.ZipCode;
                accountant.Country = Accountant.Country;
                accountant.State = Accountant.State;
                accountant.City = Accountant.City;
                User.PhoneNumber = Accountant.PhoneNumber;
                User.Email = Accountant.Email;
               
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
