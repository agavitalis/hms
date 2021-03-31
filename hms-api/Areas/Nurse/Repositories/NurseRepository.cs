using AutoMapper;
using HMS.Areas.Interfaces.Nurse;
using HMS.Areas.Nurse.Dtos;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Nurse.Repositories
{
   

    public class NurseRepository : INurse
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public NurseRepository(IMapper mapper, ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public PagedList<NurseDtoForView> GetNurses(PaginationParameter paginationParameter)
        {
            var nurses = _applicationDbContext.NurseProfiles.Include(n => n.Nurse).OrderBy(n => n.Nurse.FirstName).ToList();
            var nursesToReturn = _mapper.Map<IEnumerable<NurseDtoForView>>(nurses);
            return PagedList<NurseDtoForView>.ToPagedList(nursesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<object> GetNurse(string NurseId)
        {
            var nurse = await _applicationDbContext.NurseProfiles.Where(a => a.NurseId == NurseId).Include(a => a.Nurse).FirstOrDefaultAsync();
            var nurseToReturn = _mapper.Map<NurseDtoForView>(nurse);
            return nurseToReturn;
        } 

        public async Task<bool> EditBasicInfo(NurseBasicInfoDtoForEdit nurse)
        {
            //check if this guy has a profile already
            var nurseProfile = await _applicationDbContext.NurseProfiles.FirstOrDefaultAsync(a => a.NurseId == nurse.NurseId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == nurse.NurseId);
            // Validate account profile is not null---has no a profile yet
            if (nurseProfile == null)
            {
                var profile = new NurseProfile()
                {
                    FullName = nurse.FirstName + " " + nurse.LastName,

                    Age = nurse.Age,
                    DateOfBirth = nurse.DateOfBirth,
                    Gender = nurse.Gender,
                    NurseId = nurse.NurseId
                };

                User.FirstName = nurse.FirstName;
                User.LastName = nurse.LastName;
                User.OtherNames = nurse.OtherNames;

                _applicationDbContext.NurseProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                User.FirstName = nurse.FirstName;
                User.LastName = nurse.LastName;
                User.OtherNames = nurse.OtherNames;
                nurseProfile.FullName = nurse.FirstName + " " + nurse.LastName;
                nurseProfile.Age = nurse.Age;
                nurseProfile.Gender = nurse.Gender;
                nurseProfile.DateOfBirth = nurse.DateOfBirth;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditContactDetails(NurseContactDetailsDtoForEdit Nurse)
        {
            //check if this guy has a profile already
            var nurse = await _applicationDbContext.NurseProfiles.FirstOrDefaultAsync(d => d.NurseId == Nurse.NurseId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Nurse.NurseId);
            // Validate patient is not null---has no profile yet
            if (nurse == null)
            {
                var profile = new NurseProfile()
                {
                    Address = Nurse.Address,
                    ZipCode = Nurse.ZipCode,
                    Country = Nurse.Country,
                    State = Nurse.State,
                    City = Nurse.City,
                    NurseId = Nurse.NurseId
                };

                User.PhoneNumber = Nurse.PhoneNumber;
                User.Email = Nurse.Email;
                _applicationDbContext.NurseProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                nurse.Address = Nurse.Address;
                nurse.ZipCode = Nurse.ZipCode;
                nurse.Country = Nurse.Country;
                nurse.State = Nurse.State;
                nurse.City = Nurse.City;
                User.PhoneNumber = Nurse.PhoneNumber;
                User.Email = Nurse.Email;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditProfilePictureAsync(NurseProfilePictureDtoForEdit Nurse)
        {
            var nurse = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Nurse.NurseId);

            // Validate doctor is not null
            if (nurse != null)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var folderToSaveIn = "wwwroot/Images/";
                var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                var absoluteFilePath = "";

                //var fileName = Path.GetFileNameWithoutExtension(patientProfile.ProfilePicture.FileName);
                string extension = Path.GetExtension(Nurse.ProfilePicture.FileName);

                if (ImageValidator.FileSize(_configuration, Nurse.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                {
                    if (Nurse.ProfilePicture != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(pathToSave, Nurse.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                        {
                            await Nurse.ProfilePicture.CopyToAsync(fileStream);
                            absoluteFilePath = fileStream.Name;
                        }

                        // Make changes to the user profile picture
                        nurse.ProfileImageUrl = Path.GetRelativePath(rootPath, absoluteFilePath);

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
