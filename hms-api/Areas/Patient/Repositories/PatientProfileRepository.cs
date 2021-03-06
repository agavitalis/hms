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

        public async Task<int> GetPatientCountAsync() => await _applicationDbContext.PatientProfiles.CountAsync();

        public async Task<IEnumerable<PatientProfile>> GetPatientsAsync()
        {
            var patients = await _applicationDbContext.PatientProfiles.Include(p => p.Patient).ToListAsync();
           // return _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return patients;

        }

        public async Task<object> GetPatientsByDoctorAsync(string DoctorId) => await _applicationDbContext.MyPatients.Include(p => p.Patient).Where(p => p.DoctorId == DoctorId).OrderBy(p => p.Patient.FirstName).ToListAsync();
        public async Task<PatientProfile> GetPatientByIdAsync(string patientId) => await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == patientId).Include(p => p.Patient).Include(p => p.File).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
      
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
                                await PatientProfile.ProfilePicture.CopyToAsync(fileStream);
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
       

        public async Task<dynamic> GetPatientAppointmentByIdAsync(string patientId)
        {
            var apponintments = await _applicationDbContext.DoctorAppointments.Where(p => p.PatientId == patientId)
                                        .Select(x => new  {patient = x.Patient, apponintment = x}).FirstAsync();

           

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

            return PagedList<PatientDtoForView>.ToPagedList(patientToReurn.AsQueryable(), 1, 200);
        }

        public async Task<object> GetPatientHealthHistory(string PatientId)
        {
            var profile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == PatientId).Include(p => p.Patient).Select(p => new
            {
                FileNumber = p.FileNumber,
                Age = p.Age,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                Address = p.Address,
                ZipCode = p.ZipCode,
                City = p.City,
                State = p.State,
                Country = p.Country,
                BloodGroup = p.BloodGroup,
                Genotype = p.GenoType,
                Allergies = p.Allergies,
                Disabilities = p.Disabilities,
                Diabetic = p.Diabetic,
                PatientId = p.PatientId,
                Firstname = p.Patient.FirstName,
                Lastname = p.Patient.LastName,
                OtherNames = p.Patient.OtherNames,
                Email = p.Patient.Email,
                PhoneNumber = p.Patient.PhoneNumber
                
            }).FirstOrDefaultAsync();
            var clerking = await _applicationDbContext.DoctorClerkings.Include(c => c.Patient).Where(c => c.PatientId == PatientId).OrderByDescending(c => c.Patient.FirstName).Select(c => new
            {
                SocialHistory = c.SocialHistory,
                FamilyHistory = c.FamilyHistory,
                MedicalHistory = c.MedicalHistory,
                LastCountryVisited = c.LastCountryVisited,
                DateOfVisitation = c.DateOfVisitation,
                PresentingComplaints = c.PresentingComplaints,
                HistoryOfPresentingComplaints = c.HistoryOfPresentingComplaints,
                ReviewOfSystem = c.ReviewOfSystem,
                PhysicalExamination = c.PhysicalExamination,
                Diagnosis = c.Diagnosis,
                TreatmentPlan = c.TreatmentPlan,
                ObstetricsAndGynecology = c.ObstetricsAndGynecology,
                Prescription = c.Prescription,
                DateOfClerking = c.DateOfClerking,
                DoctorName = c.Doctor.FirstName + " " + c.Doctor.LastName,
                DoctorPhoneNumber = c.Doctor.PhoneNumber,
        
               

            }).ToListAsync();
            var preConsultation = await _applicationDbContext.PatientPreConsultation.Include(c => c.Patient).Where(p => p.PatientId == PatientId).OrderByDescending(c => c.Patient.FirstName).Select(p => new
            {
                
                BloodPressure = p.BloodPressure,
                Pulse = p.Pulse,
                Respiration = p.Respiration,
                SPO2 = p.SPO2,
                Temperature = p.Temperature,
                Weight = p.Weight,
                Height = p.Height,
                CalculatedBMI = p.CalculatedBMI,
                Date = p.Date,
            }).ToListAsync();


            var patientHistory = new PatientHealthHistoryViewModel
            {
                Profile = profile,
                Clerking = clerking,
                PreConsultation = preConsultation
            };
            return patientHistory;
        }

        public PagedList<PatientDtoForView> GetPatients(PaginationParameter paginationParameter)
        {
            var patients = _applicationDbContext.PatientProfiles.Include(d => d.Patient).OrderByDescending(c => c.Patient.FirstName).ToList();
            var patientsToReturn = _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return PagedList<PatientDtoForView>.ToPagedList(patientsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<PatientDtoForView> GetPatient(string PatientId)
        {
            var patient = await _applicationDbContext.PatientProfiles.Where(d => d.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Account).ThenInclude(a => a.HealthPlan).FirstOrDefaultAsync();
            var patientToReturn = _mapper.Map<PatientDtoForView>(patient);
            return patientToReturn;
        }
    }
}
