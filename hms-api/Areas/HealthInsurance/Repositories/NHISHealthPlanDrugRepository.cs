
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class NHISHealthPlanDrugRepository : INHISHealthPlanDrug
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NHISHealthPlanDrugRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }
        public async Task<bool> CreateHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug)
        {
            try
            {
                if (HealthPlanDrug == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanDrugs.Add(HealthPlanDrug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug)
        {
            try
            {
                if (HealthPlanDrug == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanDrugs.Remove(HealthPlanDrug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NHISHealthPlanDrug> GetHealthPlanDrug(string HealthPlanDrugId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(d => d.Id == HealthPlanDrugId).FirstOrDefaultAsync();

        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugs() => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).ToListAsync();


        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugsByDrug(string DrugId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(dp => dp.DrugId == DrugId).ToListAsync();

        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugsByHealthPlan(string HealthPlanId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(dp => dp.NHISHealthPlanId == HealthPlanId).ToListAsync();


        public async Task<bool> UpdateHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug)
        {
            try
            {
                if (HealthPlanDrug == null)
                {

                    return false;
                }

                _applicationDbContext.NHISHealthPlanDrugs.Update(HealthPlanDrug);
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
