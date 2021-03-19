using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    public class HMOSubUserGroupHealthPlanRepository : IHMOSubUserGroupHealthPlan
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public HMOSubUserGroupHealthPlanRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateHMOSubUserGroupHealthPlan(HMOSubUserGroupHealthPlan hMOSubUserGroupHealthplan)
        {
            try
            {
                if (hMOSubUserGroupHealthplan == null)
                {
                    return false;
                }

                _applicationDbContext.HMOSubUserGroupHealthPlans.Add(hMOSubUserGroupHealthplan);
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
