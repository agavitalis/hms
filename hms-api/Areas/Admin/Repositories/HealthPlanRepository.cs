using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class HealthPlanRepository : IHealthPlan
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HealthPlanRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<HealthPlan>> GetAllHealthPlan() => await _applicationDbContext.HealthPlans.ToListAsync();

        public async Task<HealthPlan> GetHealthPlanByIdAsync(int id)
        {
            try
            {
                var plan = await _applicationDbContext.HealthPlans.FindAsync(id);

                return plan;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<bool> InsertHealthPlan(HealthPlan plan)
        {
            try
            {
                if(plan == null)
                {
                    return false;
                }

                _applicationDbContext.HealthPlans.Add(plan);
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
