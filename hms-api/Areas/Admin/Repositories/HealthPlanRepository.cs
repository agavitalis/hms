using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
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

        public Task<IEnumerable<HealthPlan>> GetAllHealthPlan()
        {
            throw new NotImplementedException();
        }

        // public Task<IEnumerable<HealthPlan>> GetAllHealthPlan() => _applicationDbContext.

        public Task<HealthPlan> GetHealthPlanByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertHealthPlan(HealthPlan plan)
        {
            throw new NotImplementedException();
        }
    }
}
