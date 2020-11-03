﻿using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;

namespace HMS.Areas.Admin.Repositories
{
    public class RegisterRepository : IRegister
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RegisterRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<File> CreateFile(string accountId)
        {

            if (accountId != null)
            {
                var fileNumber =  "HMS-1";
                var lastPatientFile = _applicationDbContext.Files
               .OrderByDescending(x => x.DateCreated)
               .FirstOrDefault();

                if (lastPatientFile != null)
                {
                    string lastFileNumber = lastPatientFile.FileNumber;
                    string[] fileNumberArray = lastFileNumber.Split('-');

                    int lastNumber = int.Parse(fileNumberArray[1]) + 1;

                    fileNumber = "HMS-" + lastNumber.ToString();
                }
              
                //create this file and send it back to me
                var file = new File()
                {
                    AccountId = accountId,
                    FileNumber = fileNumber
                };

                _applicationDbContext.Files.Add(file);
                await _applicationDbContext.SaveChangesAsync();

                return file;
            }

            return null;

        }


        public async Task<bool> RegisterPatient(ApplicationUser patient, File file, Account account)
        {

            var newApplicationUser = new ApplicationUser()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                UserName = patient.Email,
                UserType = "Patient"
            };

            var createUser = await _userManager.CreateAsync(newApplicationUser, "Patient1@test");

            if (createUser.Succeeded)
            {
                //assign him to this role
                await _userManager.AddToRoleAsync(newApplicationUser, "Patient");

                // then create his profile and update his subscription plans

                var profile = new PatientProfile()
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,

                    FileId = file.Id,
                    FileNumber = file.FileNumber,

                    PatientId = newApplicationUser.Id,

                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"

                };


                _applicationDbContext.PatientProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }


            return true;

        }

        public async Task<bool> GenerateInvoice(RegistrationInvoice invoice)
        {
            try
            {
                if (invoice == null)
                {
                    return false;
                }

                _applicationDbContext.RegistrationInvoices.Add(invoice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GenerateInvoiceNumber()
        {
            try
            {
                var invoiceNumber = "HMS-1";
                var lastRegistrationInvoice = _applicationDbContext.RegistrationInvoices
               .OrderByDescending(x => x.DateGenerated)
               .FirstOrDefault();

                if (lastRegistrationInvoice != null)
                {
                    string lastInvoiceNumber = lastRegistrationInvoice.InvoiceNumber;
                    string[] invoiceNumberArray = lastInvoiceNumber.Split('-');

                    int lastNumber = int.Parse(invoiceNumberArray[1]) + 1;

                    invoiceNumber = "HMS-" + lastNumber.ToString();
                }
                return invoiceNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GenerateReferenceNumber()
        {
            try
            {
                var referenceNumber = "HMS-1";
                var lastRegistrationInvoice = _applicationDbContext.RegistrationInvoices
               .OrderByDescending(x => x.DateGenerated)
               .FirstOrDefault();

                if (lastRegistrationInvoice != null)
                {
                    string lastReferenceNumber = lastRegistrationInvoice.ReferenceNumber;
                    string[] referenceNumberArray = lastReferenceNumber.Split('-');

                    int lastNumber = int.Parse(referenceNumberArray[1]) + 1;

                    referenceNumber = "HMS-" + lastNumber.ToString();
                }
                return referenceNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
