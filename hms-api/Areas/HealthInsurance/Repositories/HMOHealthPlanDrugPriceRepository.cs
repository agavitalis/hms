using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    public class HMOHealthPlanDrugPriceRepository : IHMOHealthPlanDrugPrice
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HMOHealthPlanDrugPriceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
      
        }
        public async Task<bool> CreateDrugPrice(HMOHealthPlanDrugPrice DrugPrice)
        {
            try
            {
                if (DrugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanDrugPrices.Add(DrugPrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDrugPrice(HMOHealthPlanDrugPrice DrugPrice)
        {
            try
            {
                if (DrugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanDrugPrices.Remove(DrugPrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOHealthPlanDrugPrice> GetDrugPrice(string DrugPriceId) => await _applicationDbContext.HMOHealthPlanDrugPrices.Include(dp => dp.Drug).Include(dp => dp.HMOHealthPlan).Where(d => d.Id == DrugPriceId).FirstOrDefaultAsync();
    
        public async Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPrices() => await _applicationDbContext.HMOHealthPlanDrugPrices.Include(dp => dp.Drug).Include(dp => dp.HMOHealthPlan).ToListAsync();



        public async Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPricesByDrug(string DrugId) => await _applicationDbContext.HMOHealthPlanDrugPrices.Include(dp => dp.Drug).Include(dp => dp.HMOHealthPlan).Where(dp => dp.DrugId == DrugId).ToListAsync();
        

        public async Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPricesByHealthPlan(string HealthPlanId) => await _applicationDbContext.HMOHealthPlanDrugPrices.Include(dp => dp.Drug).Include(dp => dp.HMOHealthPlan).Where(dp => dp.HMOHealthPlanId == HealthPlanId).ToListAsync();
      

        public async Task<bool> UpdateDrugPrice(HMOHealthPlanDrugPrice DrugPrice)
        {
            try
            {
                if (DrugPrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanDrugPrices.Update(DrugPrice);
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
