using AutoMapper;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugPriceRepository : IDrugPrice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        public DrugPriceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateDrugPrice(DrugPrice drugPrice)
        {
            try
            {
                if (drugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.DrugPrices.Add(drugPrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDrugPrice(DrugPrice drugPrice)
        {
            try
            {
                if (drugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.DrugPrices.Remove(drugPrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DrugPrice> GetDrugPrice(string DrugPriceId) => await _applicationDbContext.DrugPrices.FindAsync(DrugPriceId);
        

        public async Task<IEnumerable<DrugPrice>> GetDrugPrices() => await _applicationDbContext.DrugPrices.Include(dp => dp.Drug).Include(dp => dp.HealthPlan).ToListAsync();
        

        public async Task<IEnumerable<DrugPrice>> GetDrugPricesByDrug(string DrugId) => await _applicationDbContext.DrugPrices.Include(dp => dp.Drug).Include(dp => dp.HealthPlan).Where(dp => dp.DrugId == DrugId).ToListAsync();


        public async Task<IEnumerable<DrugPrice>> GetDrugPricesByHealthPlan(string HealthPlanId) => await _applicationDbContext.DrugPrices.Include(dp => dp.Drug).Include(dp => dp.HealthPlan).Where(dp => dp.HealthPlanId == HealthPlanId).ToListAsync();

      

        public async Task<bool> UpdateDrugPrice(DrugPrice drugPrice)
        {
            try
            {
                if (drugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.DrugPrices.Update(drugPrice);
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
