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
using HMS.Services.Helpers;

namespace HMS.Areas.Admin.Repositories
{
    public class RegisterRepository : IRegister
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;

        public RegisterRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, IMapper mapper, ITransactionLog transaction, IAccount account)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _transaction = transaction;
            _account = account;
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


        public async Task<string> RegisterPatient(ApplicationUser patient, File file, Account account)
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

                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}",
                };


                _applicationDbContext.PatientProfiles.Add(profile);
                await _applicationDbContext.SaveChangesAsync();

                return newApplicationUser.Id;
            }

            var errorMessage = "";
            foreach (var item in createUser.Errors)
            {
                errorMessage = item.Description;
            }

            return errorMessage;

        }

        public async Task<object> GetPatientRegistrationInvoice(string patientId)
        {
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.PatientId == patientId).Include(i => i.HealthPlan).FirstOrDefaultAsync();
            return _mapper.Map<DtoForPatientRegistrationInvoice>(invoice);
        }

        public async Task<int> PayRegistrationFee(PatientRegistrationPaymentDto paymentDetails)
        {
            string transactionType = "Credit";
            string invoiceType = "Registration";
            DateTime transactionDate = DateTime.Now;
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == paymentDetails.PatientId).FirstOrDefaultAsync();
            var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.InvoiceNumber == paymentDetails.InvoiceNumber && i.PatientId == patient.PatientId).FirstOrDefaultAsync();
            var initiator = await _applicationDbContext.ApplicationUsers.Where(a => a.Id == paymentDetails.InitiatorId).FirstOrDefaultAsync();
            if (initiator != null)
            {


                if (patient != null)
                {
                    if (invoice != null)
                    {
                        if (invoice.Amount != paymentDetails.Amount)
                        {
                            return 1;
                        }

                        var invoiceToUpdate = _mapper.Map<RegistrationInvoice>(invoice);
                        invoiceToUpdate.DatePaid = DateTime.Now;
                        invoiceToUpdate.PaymentMethod = paymentDetails.PaymentMethod;
                        invoiceToUpdate.PaymentStatus = "Paid";
                        invoiceToUpdate.TransactionReference = paymentDetails.transactionReference;

                        _applicationDbContext.RegistrationInvoices.Update(invoiceToUpdate);

                       
                        var res = await _applicationDbContext.SaveChangesAsync();
                        if (res == 1)
                        {
                            await _transaction.LogTransaction(invoice.Amount, transactionType, invoiceType, invoice.Id, paymentDetails.PaymentMethod, transactionDate, patient.PatientId, paymentDetails.InitiatorId);
                            return 0;
                        }
                        return 2;
                    }
                    return 3;
                }
                return 4;
            }
            return 5;
        }

        public async Task<int> PayRegistrationFeeWithAccount(PatientRegistrationPaymentDto paymentDetails)
        {
            string transactionType = "Credit";
            string invoiceType = "Registration";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";
           
            string paymentMethod = null;


            DateTime transactionDate = DateTime.Now;
            var patient = await _applicationDbContext.PatientProfiles.Include(p => p.Account).Where(p => p.PatientId == paymentDetails.PatientId).FirstOrDefaultAsync();
            
            
            if (patient != null)
            {
                var invoice = await _applicationDbContext.RegistrationInvoices.Where(i => i.InvoiceNumber == paymentDetails.InvoiceNumber && i.PatientId == patient.PatientId).FirstOrDefaultAsync();
               
                if (invoice != null)
                {
                    if (patient.Account.AccountBalance > paymentDetails.Amount)
                    {
                        if (invoice.Amount != paymentDetails.Amount)
                        {
                            return 1;
                        }

                        var invoiceToUpdate = _mapper.Map<RegistrationInvoice>(invoice);
                        invoiceToUpdate.DatePaid = DateTime.Now;
                    
                        invoiceToUpdate.PaymentMethod = paymentDetails.PaymentMethod;
                        invoiceToUpdate.PaymentStatus = "Paid";
                        invoiceToUpdate.TransactionReference = paymentDetails.transactionReference;

                        _applicationDbContext.RegistrationInvoices.Update(invoiceToUpdate);

                   

                        
                        var account = await _applicationDbContext.Accounts.FirstOrDefaultAsync(s => s.Id == patient.AccountId);
                        account.AccountBalance -= paymentDetails.Amount;

                        var accountInvoiceToCreate = new AccountInvoice();

                        accountInvoiceToCreate = new AccountInvoice()
                        {
                            Amount = paymentDetails.Amount,
                            GeneratedBy = paymentDetails.InitiatorId,
                            PaymentMethod = paymentDetails.PaymentMethod,
                            TransactionReference = paymentDetails.transactionReference,
                            AccountId = account.Id,
                        };

                        var accountInvoice = await _account.CreateAccountInvoice(accountInvoiceToCreate);


                        var res = await _applicationDbContext.SaveChangesAsync();
                        if (res == 0)
                        {
                            await _transaction.LogTransaction(invoice.Amount, transactionType, invoiceType, invoice.Id, paymentDetails.PaymentMethod, transactionDate, patient.PatientId, paymentDetails.InitiatorId);
                            await _transaction.LogAccountTransactionAsync(invoice.Amount, accountTransactionType, accountInvoiceType, accountInvoice.Id, paymentMethod, transactionDate, patient.Account.Id, paymentDetails.InitiatorId);
                            return 0;
                        }
                        return 2;
                    }
                    return 5;
                }
                return 3;

            }
            return 4;
        }

        public async Task<RegistrationInvoice> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy, string patientId)
        {
            var invoiceToGenerate = new RegistrationInvoice()
            {
                Amount = amount,
                HealthPlanId = healthPlanId,
                GeneratedBy = generatedBy,
                PatientId = patientId
            };

            _applicationDbContext.RegistrationInvoices.Add(invoiceToGenerate);
            await _applicationDbContext.SaveChangesAsync();
            return invoiceToGenerate;
        }

        public async Task<RegistrationInvoice> GetRegistrationInvoice(string PatientId) => await _applicationDbContext.RegistrationInvoices.Where(i => i.PatientId == PatientId).Include(i => i.Patient).FirstOrDefaultAsync();

        public async Task<IEnumerable<RegistrationInvoice>> GetRegistrationInvoices() => await _applicationDbContext.RegistrationInvoices.Include(i => i.Patient).ToListAsync();

        public PagedList<RegistrationInvoiceDtoForView> GetRegistrationInvoicesPagnation(PaginationParameter paginationParameter)
        {
            var registrationInvoice = _applicationDbContext.RegistrationInvoices.Include(i => i.Patient).ToList();
            var registrationInvoicesToReturn = _mapper.Map<IEnumerable<RegistrationInvoiceDtoForView>>(registrationInvoice);
            return PagedList<RegistrationInvoiceDtoForView>.ToPagedList(registrationInvoicesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
