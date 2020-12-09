using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugRepository : IDrug
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DrugRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<int> GetDrugCount() => await _applicationDbContext.Drugs.CountAsync();
        public async Task<Drug> GetDrug(string Id) => await _applicationDbContext.Drugs.FindAsync(Id);

        public async Task<IEnumerable<Drug>> GetDrugs() => await _applicationDbContext.Drugs.ToListAsync();

           
        public async Task<bool> CreateDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }

                _applicationDbContext.Drugs.Add(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }

                _applicationDbContext.Drugs.Update(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }

                _applicationDbContext.Drugs.Remove(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
