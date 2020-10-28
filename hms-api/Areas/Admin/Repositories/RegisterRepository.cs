﻿﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using HMS.Models;
using HMS.Areas.Doctor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Services.Helpers;
using HMS.Areas.Patient.Models;

namespace HMS.Areas.Admin.Repositories
{
    public class RegisterRepository : IRegister
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public RegisterRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public Task<(bool, string)> AddNewPatient()
        {
            throw new NotImplementedException();
        }

     
        public async Task<File> GenerateFileNumber(FileDtoForCreate fileToCreate)
        {
            try
            {
                if (fileToCreate != null)
                {
                    var file = _mapper.Map<File>(fileToCreate);

                    _applicationDbContext.Files.Add(file);
                    await _applicationDbContext.SaveChangesAsync();

                    return file;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }         
        }


        public async Task<Models.Account> GetAccountById(string Id)
        {
            try
            {
                if(string.IsNullOrEmpty(Id))
                {
                    return null;
                }

                var account = await _applicationDbContext.Accounts.FindAsync(Id);

                return account;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Models.Account>> GetAllAccounts(PaginationParameter pagination)
        {
             var result = _applicationDbContext.Accounts.Where(x => x.IsActive == true).AsQueryable();

            return PagedList<Models.Account>.Create(result, pagination.PageSize, pagination.PageNumber);
        }
        
        public async Task<IEnumerable<PatientProfile>> GetPatientsInAccount(string acctId)
        {
            try
            {
                var patients = await _applicationDbContext.PatientProfiles.Where(x => x.AccountId == acctId).ToListAsync();

                return patients;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public async Task<Models.Account> InsertAccount(Models.Account account)
        {
            try
            {
                if(account == null)
                {
                    return null;
                }

                _applicationDbContext.Accounts.Add(account);
                await _applicationDbContext.SaveChangesAsync();

                return account;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertPatient(PatientProfile patient)
        {
            if(patient == null)
            {
                return false;
            }

            _applicationDbContext.PatientProfiles.Add(patient);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
