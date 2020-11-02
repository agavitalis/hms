﻿using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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
        public AccountRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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

        public async Task<IEnumerable<Account>> GetAllAccounts() => await _applicationDbContext.Accounts.ToListAsync();

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            try
            {
                var account = await _applicationDbContext.Accounts.FindAsync(id);

                return account;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAccount(Account account)
        {
            try
            {
                if (account == null)
                {
                    return false;
                }

                _applicationDbContext.Accounts.Update(account);
                await _applicationDbContext.SaveChangesAsync();

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

        public IEnumerable<Account> GetAllAccountsPaginated(PaginationParameter pagination)
        {
            var result = _applicationDbContext.Accounts.Where(x => x.IsActive == true).AsQueryable();
            return PagedList<Account>.Create(result, pagination.PageSize, pagination.PageNumber);
        }

        public async Task<IEnumerable<PatientProfile>> GetPatientsInAccount(string accounttId)
        {
            try
            {
                var patients = await _applicationDbContext.PatientProfiles.Where(x => x.AccountId == accounttId).ToListAsync();

                return patients;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
