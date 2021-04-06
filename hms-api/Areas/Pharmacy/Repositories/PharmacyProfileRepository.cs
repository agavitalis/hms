using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
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
        private readonly IMapper _mapper;

        public PharmacyProfileRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<object> GetPharmacyProfileByIdAsync(string Id) => await _applicationDbContext.PharmacyProfiles.Where(p => p.PharmacistId == Id).Include(p => p.Pharmacist).ToListAsync();
        
        public async Task<object> GetAllPharmacyAsync() => await _applicationDbContext.PharmacyProfiles.Include(p => p.Pharmacist).ToListAsync();
                      

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

        public async Task<bool> EditPharmacistBasicInfoAsync(EditPharmacistBasicInfoViewModel Pharmacist)
        {
            //check if this guy has a profile already
            var pharmacist = await _applicationDbContext.PharmacyProfiles.FirstOrDefaultAsync(a => a.PharmacistId == Pharmacist.PharmacistId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Pharmacist.PharmacistId);
            // Validate account profile is not null---has no a profile yet
            if (pharmacist == null)
            {
                var profile = new PharmacyProfile()
                {
                    FullName = Pharmacist.FirstName + " " + Pharmacist.LastName,

                    Age = Pharmacist.FirstName + " "+ Pharmacist.LastName,
                    DateOfBirth = Pharmacist.DateOfBirth,
                    Gender = Pharmacist.Gender,
                    PharmacistId = Pharmacist.PharmacistId
                };

                User.FirstName = Pharmacist.FirstName;
                User.LastName = Pharmacist.LastName;
                User.OtherNames = Pharmacist.OtherNames;

                _applicationDbContext.PharmacyProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = Pharmacist.FirstName;
                User.LastName = Pharmacist.LastName;
                User.OtherNames = Pharmacist.OtherNames;
                pharmacist.FullName = Pharmacist.FirstName + " " + Pharmacist.LastName;
                pharmacist.Age = Pharmacist.Age;
                pharmacist.Gender = Pharmacist.Gender;
                pharmacist.DateOfBirth = Pharmacist.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditPharmacistContactDetailsAsync(EditPharmacistContactDetailsViewModel Pharmacist)
        {
            //check if this guy has a profile already
            var pharmacist = await _applicationDbContext.PharmacyProfiles.FirstOrDefaultAsync(d => d.PharmacistId == Pharmacist.PharmacistId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Pharmacist.PharmacistId);
            // Validate patient is not null---has no profile yet
            if (pharmacist == null)
            {
                var profile = new AccountantProfile()
                {
                    Address = Pharmacist.Address,
                    ZipCode = Pharmacist.ZipCode,
                    Country = Pharmacist.Country,
                    State = Pharmacist.State,
                    City = Pharmacist.City,
                    AccountantId = Pharmacist.PharmacistId
                };

                User.PhoneNumber = Pharmacist.PhoneNumber;
                User.Email = Pharmacist.Email;
                
                _applicationDbContext.AccountantProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                pharmacist.Address = Pharmacist.Address;
                pharmacist.ZipCode = Pharmacist.ZipCode;
                pharmacist.Country = Pharmacist.Country;
                pharmacist.State = Pharmacist.State;
                pharmacist.City = Pharmacist.City;
                User.PhoneNumber = Pharmacist.PhoneNumber;
                User.Email = Pharmacist.Email;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public PagedList<PharmacistDtoForView> GetPharmacists(PaginationParameter paginationParameter)
        {
            var pharmacists = _applicationDbContext.PharmacyProfiles.Include(p => p.Pharmacist).OrderBy(p => p.Pharmacist.FirstName).ToList();
            var pharmacistsToReturn = _mapper.Map<IEnumerable<PharmacistDtoForView>>(pharmacists);
            return PagedList<PharmacistDtoForView>.ToPagedList(pharmacistsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<PharmacistDtoForView> GetPharmacist(string PharmacistId)
        {
            var pharmacist = await _applicationDbContext.PharmacyProfiles.Where(a => a.PharmacistId == PharmacistId).Include(a => a.Pharmacist).FirstOrDefaultAsync();
            var pharmacistToReturn = _mapper.Map<PharmacistDtoForView>(pharmacist);
            return pharmacistToReturn;
        }
    }
}
