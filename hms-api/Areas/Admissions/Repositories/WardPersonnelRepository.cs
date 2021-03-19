using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
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

namespace HMS.Areas.Admissions.Repositories
{
    public class WardPersonnelRepository : IWardPersonnel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public WardPersonnelRepository(IMapper mapper, ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<object> GetWardPersonnel(string WardPersonnelId)
        {
            var wardPersonnel = await _applicationDbContext.WardPersonnelProfiles.Where(a => a.WardPersonnelId == WardPersonnelId).Include(a => a.WardPersonnel).FirstOrDefaultAsync();
            var wardPersonnelToReturn = _mapper.Map<WardPersonnelDtoForView>(wardPersonnel);
            return wardPersonnelToReturn;
        }

        public PagedList<WardPersonnelDtoForView> GetWardPersonnels(PaginationParameter paginationParameter)
        {
            var wardPersonnels = _applicationDbContext.WardPersonnelProfiles.Include(w => w.WardPersonnel).ToList();
            var wardPersonnelsToReturn = _mapper.Map<IEnumerable<WardPersonnelDtoForView>>(wardPersonnels);
            return PagedList<WardPersonnelDtoForView>.ToPagedList(wardPersonnelsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
            
        }

        public async Task<bool> UpdateBasicInfo(WardPersonnelBasicInfoDtoForUpdate WardPersonnel)
        {
            //check if this guy has a profile already
            var wardPersonnelProfile = await _applicationDbContext.WardPersonnelProfiles.FirstOrDefaultAsync(a => a.WardPersonnelId == WardPersonnel.WardPersonnelId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == WardPersonnel.WardPersonnelId);
            // Validate account profile is not null---has no a profile yet
            if (wardPersonnelProfile == null)
            {
                var profile = new WardPersonnelProfile()
                {
                    FullName = WardPersonnel.FirstName + " " + WardPersonnel.LastName,

                    Age = WardPersonnel.Age,
                    DateOfBirth = WardPersonnel.DateOfBirth,
                    Gender = WardPersonnel.Gender,
                    WardPersonnelId = WardPersonnel.WardPersonnelId
                };

                User.FirstName = WardPersonnel.FirstName;
                User.LastName = WardPersonnel.LastName;
                User.OtherNames = WardPersonnel.OtherNames;

                _applicationDbContext.WardPersonnelProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = WardPersonnel.FirstName;
                User.LastName = WardPersonnel.LastName;
                User.OtherNames = WardPersonnel.OtherNames;
                wardPersonnelProfile.FullName = WardPersonnel.FirstName + " " + WardPersonnel.LastName;
                wardPersonnelProfile.Age = WardPersonnel.Age;
                wardPersonnelProfile.Gender = WardPersonnel.Gender;
                wardPersonnelProfile.DateOfBirth = WardPersonnel.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateContactDetails(WardPersonnelContactDetailsDtoForUpdate WardPersonnel)
        {
            //check if this guy has a profile already
            var wardPersonnelProfile = await _applicationDbContext.WardPersonnelProfiles.FirstOrDefaultAsync(d => d.WardPersonnelId == WardPersonnel.WardPersonnelId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == WardPersonnel.WardPersonnelId);
            // Validate patient is not null---has no profile yet
            if (wardPersonnelProfile == null)
            {
                var profile = new NurseProfile()
                {
                    Address = WardPersonnel.Address,
                    ZipCode = WardPersonnel.ZipCode,
                    Country = WardPersonnel.Country,
                    State = WardPersonnel.State,
                    City = WardPersonnel.City,
                    NurseId = WardPersonnel.WardPersonnelId
                };

                User.PhoneNumber = WardPersonnel.PhoneNumber;
                User.Email = WardPersonnel.Email;
                _applicationDbContext.NurseProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                wardPersonnelProfile.Address = WardPersonnel.Address;
                wardPersonnelProfile.ZipCode = WardPersonnel.ZipCode;
                wardPersonnelProfile.Country = WardPersonnel.Country;
                wardPersonnelProfile.State = WardPersonnel.State;
                wardPersonnelProfile.City = WardPersonnel.City;
                User.PhoneNumber = WardPersonnel.PhoneNumber;
                User.Email = WardPersonnel.Email;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateProfilePictureAsync(WardPersonnelProfilePictureDtoForUpdate WardPersonnel)
        {
            var wardPersonnelProfile = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == WardPersonnel.WardPersonnelId);

            // Validate doctor is not null
            if (wardPersonnelProfile != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "wwwroot/Images/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(WardPersonnel.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, WardPersonnel.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (WardPersonnel.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, WardPersonnel.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            await WardPersonnel.ProfilePicture.CopyToAsync(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        wardPersonnelProfile.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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
