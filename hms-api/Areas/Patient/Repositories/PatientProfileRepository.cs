﻿using AutoMapper;
using HMS.Areas.Patient.Dtos;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
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

namespace HMS.Areas.Patient.Repositories
{
    public class PatientProfileRepository : IPatientProfile
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public PatientProfileRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration config, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
            _mapper = mapper;
        }

        
        public async Task<IEnumerable<PatientProfile>> GetPatientsAsync()
        {
            var patients = await _applicationDbContext.PatientProfiles.Include(p => p.Patient).ToListAsync();
           // return _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return patients;

        }


        public async Task<PatientProfile> GetPatientByIdAsync(string patientId)
        {
            var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == patientId).Include(p => p.Patient).Include(p => p.File).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
            return  PatientProfile;
        }
        
        public async Task<PatientProfile> GetPatientByProfileIdAsync(string patientId)
        {
            var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.Id == patientId).Include(p => p.Patient).FirstOrDefaultAsync();
            return  PatientProfile;
        }


        public async Task<bool> EditPatientProfilePictureAsync(PatientProfilePictureViewModel PatientProfile)
        {
            try
            {
                var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();

                if (PatientProfile != null)
                {


                    var rootPath = _webHostEnvironment.ContentRootPath;
                    var folderToSaveIn = "wwwroot/Images/";
                    var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                    var absoluteFilePath = "";

                    string extension = Path.GetExtension(PatientProfile.ProfilePicture.FileName);

                    if (ImageValidator.FileSize(_config, PatientProfile.ProfilePicture.Length) && ImageValidator.Filetype(extension))
                    {
                        if (PatientProfile.ProfilePicture != null)
                        {

                            using (var fileStream = new FileStream(Path.Combine(pathToSave, PatientProfile.ProfilePicture.FileName), FileMode.Create, FileAccess.Write))
                            {
                                PatientProfile.ProfilePicture.CopyToAsync(fileStream);
                                absoluteFilePath = fileStream.Name;
                            }


                            if (patient != null)
                            {
                                patient.Image = Path.GetRelativePath(rootPath, absoluteFilePath);
             
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
                    return false; 
                }

                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EditPatientBasicInfoAsync(EditPatientBasicInfoViewModel patientProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.PatientProfiles.FirstOrDefaultAsync(d => d.PatientId == patientProfile.PatientId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == patientProfile.PatientId);

            // Validate patient is not null---has no profile yet
            if (Patient == null)
            {
                var profile = new PatientProfile()
                {
                    Age = patientProfile.Age,
                    Gender = patientProfile.Gender,
                    DateOfBirth = patientProfile.DateOfBirth,
                    PatientId = patientProfile.PatientId
                    
                };
                User.FirstName = patientProfile.FirstName;
                User.LastName = patientProfile.LastName;
                User.OtherNames = patientProfile.OtherNames;
        
                _applicationDbContext.PatientProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                User.FirstName = patientProfile.FirstName;
                User.LastName = patientProfile.LastName;
                User.OtherNames = patientProfile.OtherNames;
                Patient.Age = patientProfile.Age;
                Patient.Gender = patientProfile.Gender;
                Patient.DateOfBirth = patientProfile.DateOfBirth;
               
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditPatientAddressAsync(PatientAddressViewModel patientProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.PatientProfiles.FirstOrDefaultAsync(d => d.PatientId == patientProfile.PatientId);
            var User = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == patientProfile.PatientId);
            // Validate patient is not null---has no profile yet
            if (Patient == null)
            {
                var profile = new PatientProfile()
                {
                    Address = patientProfile.Address,
                    ZipCode = patientProfile.ZipCode,
                    Country = patientProfile.Country,
                    State = patientProfile.State,
                    City = patientProfile.City,
                    PatientId = patientProfile.PatientId

                };

                User.PhoneNumber = patientProfile.PhoneNumber;
                User.Email = patientProfile.Email;
                _applicationDbContext.PatientProfiles.Add(profile);
                
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                Patient.Address = patientProfile.Address;
                Patient.ZipCode = patientProfile.ZipCode;
                Patient.Country = patientProfile.Country;
                Patient.State = patientProfile.State;
                Patient.City = patientProfile.City;
                User.PhoneNumber = patientProfile.PhoneNumber;
                User.Email = patientProfile.Email;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> EditPatientHealthAsync(PatientHealthViewModel patientProfile)
        {
            //throw new NotImplementedException();
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.PatientProfiles.FirstOrDefaultAsync(d => d.PatientId == patientProfile.PatientId);

            // Validate patient is not null---has no profile yet
            if (Patient == null)
            {
                var profile = new PatientProfile()
                {
                    BloodGroup = patientProfile.BloodGroup,
                    GenoType = patientProfile.GenoType,
                    Allergies = patientProfile.Allergies,
                    Disabilities = patientProfile.Disabilities,
                    Diabetic = patientProfile.Diabetic,
                    PatientId = patientProfile.PatientId

                };

                _applicationDbContext.PatientProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {

                Patient.BloodGroup = patientProfile.BloodGroup;
                Patient.GenoType = patientProfile.GenoType;
                Patient.Allergies = patientProfile.Allergies;
                Patient.Disabilities = patientProfile.Disabilities;
                Patient.Diabetic = patientProfile.Diabetic;                

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }
        //public async Task<dynamic> GetPatientAppointmentByIdAsync(string patientId)
        //{
        //    var apponintments = await _applicationDbContext.DoctorAppointments.Where(p => p.PatientId == patientId)
        //                                .Select(x => new  {patient = x.Patient, apponintment = x}).FirstAsync();

        public async Task<dynamic> GetPatientAppointmentByIdAsync(string patientId)
        {
            var apponintments = await _applicationDbContext.DoctorAppointments.Where(p => p.PatientId == patientId)
                                        .Select(x => new  {patient = x.Patient, apponintment = x}).FirstAsync();

            //.Join(
            //    _applicationDbContext.ApplicationUsers,
            //    appointment => appointment.PatientId,
            //    patient => patient.Id,
            //    (appointment, patient) => new { appointment, patient }
            //)

            return apponintments;
        }

        public IEnumerable<PatientDtoForView> SearchPatient(string searchParam)
        {
            if (string.IsNullOrEmpty(searchParam))
                return null;

            var result = _applicationDbContext.PatientProfiles.Where(p => p.FullName.Contains(searchParam) ||
                           p.FileNumber.Contains(searchParam) || p.AccountNumber.Contains(searchParam))
                           .OrderBy(x => x.FullName).AsQueryable();

            var patientToReurn = _mapper.Map<IEnumerable<PatientDtoForView>>(result);

            return PagedList<PatientDtoForView>.Create(patientToReurn.AsQueryable(), 1, 200);
        }


    }
}
