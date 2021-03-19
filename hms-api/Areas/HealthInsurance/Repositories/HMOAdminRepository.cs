using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
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

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class HMOAdminRepository : IHMOAdmin
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HMOAdminRepository(IMapper mapper, ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<object> GetHMOAdmin(string GetHMOAdminId)
        {
            var hmoAdmin = await _applicationDbContext.HMOAdminProfiles.Where(a => a.HMOAdminId == GetHMOAdminId).Include(a => a.HMOAdmin).FirstOrDefaultAsync();
            var hmoAdminToReturn = _mapper.Map<HMOAdminDtoForView>(hmoAdmin);
            return hmoAdminToReturn;
        }

        public PagedList<HMOAdminDtoForView> GetHMOAdmins(PaginationParameter paginationParameter)
        {
            var HMOAdmins = _applicationDbContext.HMOAdminProfiles.Include(h => h.HMOAdmin).ToList();
            var HMOAdminsToReturn = _mapper.Map<IEnumerable<HMOAdminDtoForView>>(HMOAdmins);
            return PagedList<HMOAdminDtoForView>.ToPagedList(HMOAdminsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);

        }

        public async Task<bool> UpdateBasicInfo(HMOAdminBasicInfoDtoForUpdate HMOAdmin)
        {
            //check if this guy has a profile already
            var hMOAdmin = await _applicationDbContext.HMOAdminProfiles.FirstOrDefaultAsync(a => a.HMOAdminId == HMOAdmin.HMOAdminId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == HMOAdmin.HMOAdminId);
            // Validate account profile is not null---has no a profile yet
            if (hMOAdmin == null)
            {
                var profile = new HMOAdminProfile()
                {
                    FullName = HMOAdmin.FirstName + " " + HMOAdmin.LastName,

                    Age = HMOAdmin.Age,
                    DateOfBirth = HMOAdmin.DateOfBirth,
                    Gender = HMOAdmin.Gender,
                    HMOAdminId = HMOAdmin.HMOAdminId
                };

                User.FirstName = HMOAdmin.FirstName;
                User.LastName = HMOAdmin.LastName;
                User.OtherNames = HMOAdmin.OtherNames;

                _applicationDbContext.HMOAdminProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = HMOAdmin.FirstName;
                User.LastName = HMOAdmin.LastName;
                User.OtherNames = HMOAdmin.OtherNames;
                hMOAdmin.FullName = HMOAdmin.FirstName + " " + HMOAdmin.LastName;
                hMOAdmin.Age = HMOAdmin.Age;
                hMOAdmin.Gender = HMOAdmin.Gender;
                hMOAdmin.DateOfBirth = HMOAdmin.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateContactDetails(HMOAdminContactDetailsDtoForUpdate HMOAdmin)
        {
            //check if this guy has a profile already
            var hMOAdmin = await _applicationDbContext.HMOAdminProfiles.FirstOrDefaultAsync(d => d.HMOAdminId == HMOAdmin.HMOAdminId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == HMOAdmin.HMOAdminId);
            // Validate patient is not null---has no profile yet
            if (hMOAdmin == null)
            {
                var profile = new NurseProfile()
                {
                    Address = HMOAdmin.Address,
                    ZipCode = HMOAdmin.ZipCode,
                    Country = HMOAdmin.Country,
                    State = HMOAdmin.State,
                    City = HMOAdmin.City,
                    NurseId = HMOAdmin.HMOAdminId
                };

                User.PhoneNumber = HMOAdmin.PhoneNumber;
                User.Email = HMOAdmin.Email;
                _applicationDbContext.NurseProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                hMOAdmin.Address = HMOAdmin.Address;
                hMOAdmin.ZipCode = HMOAdmin.ZipCode;
                hMOAdmin.Country = HMOAdmin.Country;
                hMOAdmin.State = HMOAdmin.State;
                hMOAdmin.City = HMOAdmin.City;
                User.PhoneNumber = HMOAdmin.PhoneNumber;
                User.Email = HMOAdmin.Email;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateProfilePictureAsync(HMOAdminProfilePictureDtoForUpdate HMOAdmin)
        {
            var hMOAdmin = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == HMOAdmin.HMOAdminId);

            // Validate doctor is not null
            if (hMOAdmin != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "wwwroot/Images/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(HMOAdmin.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, HMOAdmin.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (HMOAdmin.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, HMOAdmin.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            await HMOAdmin.ProfilePicture.CopyToAsync(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        hMOAdmin.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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
