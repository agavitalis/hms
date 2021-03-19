﻿using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class NHISHealthPlanServiceRepository : INHISHealthPlanService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NHISHealthPlanServiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }
        public async Task<bool> CreateHealthPlanService(NHISHealthPlanService HealthPlanService)
        {
            try
            {
                if (HealthPlanService == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanServices.Add(HealthPlanService);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteHealthPlanService(NHISHealthPlanService HealthPlanService)
        {
            try
            {
                if (HealthPlanService == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanServices.Remove(HealthPlanService);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NHISHealthPlanService> GetHealthPlanService(string HealthPlanServiceId) => await _applicationDbContext.NHISHealthPlanServices.Include(dp => dp.Service).Include(dp => dp.NHISHealthPlan).Where(d => d.Id == HealthPlanServiceId).FirstOrDefaultAsync();

        public async Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServices() => await _applicationDbContext.NHISHealthPlanServices.Include(dp => dp.Service).Include(dp => dp.NHISHealthPlan).ToListAsync();

        public async Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServicesByHealthPlan(string HealthPlanId) => await _applicationDbContext.NHISHealthPlanServices.Include(dp => dp.Service).Include(dp => dp.NHISHealthPlan).Where(dp => dp.NHISHealthPlanId == HealthPlanId).ToListAsync();


        public async Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServicesByService(string ServiceId) => await _applicationDbContext.NHISHealthPlanServices.Include(dp => dp.Service).Include(dp => dp.NHISHealthPlan).Where(dp => dp.ServiceId == ServiceId).ToListAsync();

        public async Task<bool> UpdateHealthPlanService(NHISHealthPlanService HealthPlanService)
        {
            try
            {
                if (HealthPlanService == null)
                {

                    return false;
                }

                _applicationDbContext.NHISHealthPlanServices.Update(HealthPlanService);
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
