﻿using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;
using HMS.Areas.Admin.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HMS.Services.Interfaces;

namespace HMS.Areas.Admin.Repositories
{
    public class RegisterRepository : IRegister
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITransactionLog _transaction;

        public RegisterRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, IMapper mapper, ITransactionLog transaction)
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
                var fileNumber = "HMS-1";
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


        public async Task<ApplicationUser> RegisterPatient(ApplicationUser patient, File file, Account account)
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

                return newApplicationUser;
            }


            return null;

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

        public async Task<bool> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy)
        {
            var invoiceToGenerate = new RegistrationInvoice()
            {
                Amount = amount,
                HealthPlanId = healthPlanId,
                GeneratedBy = generatedBy
            };

            _applicationDbContext.RegistrationInvoices.Add(invoiceToGenerate);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetPatientRegistrationInvoice(string patientId)
        {
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.PatientId == patientId).FirstOrDefaultAsync();
            return _mapper.Map<IEnumerable<DtoForPatientRegistrationInvoice>>(invoice);
        }

        public async Task<bool> PayRegistrationFee(PatientRegistrationPaymentDto paymentDetails)
        {
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.InvoiceNumber == paymentDetails.InvoiceNumber).FirstOrDefaultAsync();
            if (invoice.Amount == paymentDetails.Amount)
            {
                return false;
            }

            var invoiceToUpdate = _mapper.Map<RegistrationInvoice>(paymentDetails);
            invoiceToUpdate.DatePaid = DateTime.Now;
            var res = await UpdateRegistrationInvoice(invoiceToUpdate, paymentDetails.Description);
            if (res)
            {

            }
            return false;
        }

        public async Task<bool> UpdateRegistrationInvoice(RegistrationInvoice invoice, string description)
        {

            try
            {
                string transactionType = "Credit";
                string invoiceType = "RegistrationInvoice";
                DateTime transactionDate = DateTime.Now;
                if (invoice == null)
                {
                    return false;
                }

                _applicationDbContext.RegistrationInvoices.Update(invoice);
                var res = await _applicationDbContext.SaveChangesAsync();
                if (res == 1)
                {

                    await _transaction.LogTransaction(invoice.Amount, transactionType, invoiceType, invoice.Id, description, transactionDate);
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
