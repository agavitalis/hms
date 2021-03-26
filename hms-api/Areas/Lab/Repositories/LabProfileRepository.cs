using AutoMapper;
using HMS.Areas.Lab.Dtos;
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
        private readonly IMapper _mapper;

        public LabProfileRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<LabProfile> GetLabByIdAsync(string LabId) => await _applicationDbContext.LabProfiles.Where(l => l.LabAttendantId == LabId).Include(l => l.Lab).FirstOrDefaultAsync();
    
        public async Task<object> GetLabProfiles() => await _applicationDbContext.LabProfiles.Include(l => l.Lab).ToListAsync();
       
        public async Task<bool> EditLabProfileBasicInfoAsync(EditLabProfileBasicInfoViewModel labProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var LabProfile = await _applicationDbContext.LabProfiles.FirstOrDefaultAsync(d => d.LabAttendantId == labProfile.LabId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == labProfile.LabId);

            // Validate patient is not null---has no profile yet
            if (LabProfile == null)
            {
                var profile = new LabProfile()
                {
                    FullName = labProfile.FirstName + " " + labProfile.LastName,
                    Age = labProfile.Age,
                    Gender = labProfile.Gender,
                    DateOfBirth = labProfile.DateOfBirth,
                    LabAttendantId = labProfile.LabId

                };
                User.FirstName = labProfile.FirstName;
                User.LastName = labProfile.LastName;
                User.OtherNames = labProfile.OtherNames;

                _applicationDbContext.LabProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                User.FirstName = labProfile.FirstName;
                User.LastName = labProfile.LastName;
                User.OtherNames = labProfile.OtherNames;
                LabProfile.FullName = labProfile.FirstName + " " + labProfile.LastName;
                LabProfile.Age = labProfile.Age;
                LabProfile.Gender = labProfile.Gender;
                LabProfile.DateOfBirth = labProfile.DateOfBirth;

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

        public async Task<bool> EditLabProfileContactDetailsAsync(EditLabProfileContactDetailsViewModel LabProfile)
        {
            //check if this guy has a profile already
            var labProfile = await _applicationDbContext.LabProfiles.FirstOrDefaultAsync(d => d.LabAttendantId == LabProfile.LabId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == LabProfile.LabId);
            // Validate patient is not null---has no profile yet
            if (labProfile == null)
            {
                var profile = new LabProfile()
                {
                    Address = LabProfile.Address,
                    ZipCode = LabProfile.ZipCode,
                    Country = LabProfile.Country,
                    State = LabProfile.State,
                    City = LabProfile.City,
                    LabAttendantId = LabProfile.LabId

                };

                User.PhoneNumber = LabProfile.PhoneNumber;
                User.Email = LabProfile.Email;
                _applicationDbContext.LabProfiles.Add(profile);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                labProfile.Address = LabProfile.Address;
                labProfile.ZipCode = LabProfile.ZipCode;
                labProfile.Country = LabProfile.Country;
                labProfile.State = LabProfile.State;
                labProfile.City = LabProfile.City;
                User.PhoneNumber = LabProfile.PhoneNumber;
                User.Email = LabProfile.Email;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<LabAttendantDtoForView> GetLabAttendant(string LabId)
        {
            var labAttendant = await _applicationDbContext.LabProfiles.Where(a => a.LabAttendantId == LabId).Include(a => a.Lab).FirstOrDefaultAsync();
            var labAttendantToReturn = _mapper.Map<LabAttendantDtoForView>(labAttendant);
            return labAttendantToReturn;
        }

        public PagedList<LabAttendantDtoForView> GetLabAttendants(PaginationParameter paginationParameter)
        {
            var labAttendants = _applicationDbContext.LabProfiles.Include(l => l.Lab).ToList();
            var labAttendantsToReturn = _mapper.Map<IEnumerable<LabAttendantDtoForView>>(labAttendants);
            return PagedList<LabAttendantDtoForView>.ToPagedList(labAttendantsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
