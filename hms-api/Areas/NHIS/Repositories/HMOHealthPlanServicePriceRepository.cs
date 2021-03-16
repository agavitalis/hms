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
    public class HMOHealthPlanServicePriceRepository : IHMOHealthPlanServicePrice
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HMOHealthPlanServicePriceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public async Task<bool> CreateServicePrice(HMOHealthPlanServicePrice ServicePrice)
        {
            try
            {
                if (ServicePrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanServicePrices.Add(ServicePrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteServicePrice(HMOHealthPlanServicePrice ServicePrice)
        {
            try
            {
                if (ServicePrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanServicePrices.Remove(ServicePrice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOHealthPlanServicePrice> GetServicePrice(string ServicePriceId) => await _applicationDbContext.HMOHealthPlanServicePrices.Where(s => s.Id == ServicePriceId).Include(dp => dp.Service).Include(dp => dp.HMOHealthPlan).FirstOrDefaultAsync();
       

        public async Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePrices() => await _applicationDbContext.HMOHealthPlanServicePrices.Include(dp => dp.Service).Include(dp => dp.HMOHealthPlan).ToListAsync();


        public async Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePricesByHealthPlan(string HealthPlanId) => await _applicationDbContext.HMOHealthPlanServicePrices.Include(dp => dp.Service).Include(dp => dp.HMOHealthPlan).Where(dp => dp.HMOHealthPlanId == HealthPlanId).ToListAsync();
       

        public async Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePricesByService(string ServiceId) => await _applicationDbContext.HMOHealthPlanServicePrices.Include(dp => dp.Service).Include(dp => dp.HMOHealthPlan).Where(dp => dp.ServiceId == ServiceId).ToListAsync();
        

        public async Task<bool> UpdateServicePrice(HMOHealthPlanServicePrice ServicePrice)
        {
            try
            {
                if (ServicePrice == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanServicePrices.Update(ServicePrice);
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
