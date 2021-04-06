﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Dtos;
using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class AccountRepository : IAccount
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public AccountRepository(ApplicationDbContext applicationDbContext, IPatientProfile patientRepository, ITransactionLog transaction, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<AccountInvoice> CreateAccountInvoice(AccountInvoice accountInvoice)
        {
            try
            {
                if (accountInvoice == null)
                {
                    return null;
                }

                _applicationDbContext.AccountInvoices.Add(accountInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return accountInvoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   


        public async Task<bool> CreateAccount(Account account)
        {
            try
            {
                if (account == null)
                {
                    return false;
                }

                _applicationDbContext.Accounts.Add(account);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Account>> GetAllAccounts() => await _applicationDbContext.Accounts.Include(i => i.HealthPlan).OrderBy(a => a.Name).ToListAsync();

        public async Task<Account> GetAccountByIdAsync(string id) => await _applicationDbContext.Accounts.FindAsync(id);
        public async Task<AccountInvoice> GetAccountInvoice(string AccountInvoiceId) => await _applicationDbContext.AccountInvoices.FindAsync(AccountInvoiceId);
        public async Task<Account> GetAccountByAccountNumber(string AccountNumber) => await _applicationDbContext.Accounts.Where(a => a.AccountNumber == AccountNumber).FirstOrDefaultAsync();


        public async Task<bool> UpdateAccount(Account account)
        {
            try
            {
                if (account == null)
                {
                    return false;
                }

                _applicationDbContext.Accounts.Update(account);
                var res = await _applicationDbContext.SaveChangesAsync();
             
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAccount(Account account)
        {
            try
            {
                if (account == null)
                {
                    return false;
                }

                _applicationDbContext.Accounts.Remove(account);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public PagedList<AccountDtoForView> GetAccountsPagination(PaginationParameter paginationParameter)
        {
            var accounts = _applicationDbContext.Accounts.Include(a => a.HealthPlan).OrderBy(a => a.Name).ToList();

            var accountsToReturn = _mapper.Map<IEnumerable<AccountDtoForView>>(accounts);

            return PagedList<AccountDtoForView>.ToPagedList(accountsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<PatientDtoForView> GetPatientsInAccount(PaginationParameter paginationParameter, string AccountId)
        {
            var patients = _applicationDbContext.PatientProfiles.Include(p => p.Patient).Where(p => p.AccountId == AccountId).OrderBy(a => a.Patient.FirstName).ToList();

            var patientsToReturn = _mapper.Map<IEnumerable<PatientDtoForView>>(patients);

            return PagedList<PatientDtoForView>.ToPagedList(patientsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
