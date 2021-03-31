using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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
        private readonly IMapper _mapper;

        public NHISHealthPlanDrugRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;

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

        public async Task<int> GetNHISHealthPlanDrugCount(string NHISHealthPlanId) => await _applicationDbContext.NHISHealthPlanDrugs.Where(h => h.NHISHealthPlanId == NHISHealthPlanId).CountAsync();
      

        public async Task<NHISHealthPlanDrug> GetHealthPlanDrug(string HealthPlanDrugId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(d => d.Id == HealthPlanDrugId).FirstOrDefaultAsync();

        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugs() => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).ToListAsync();


        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugsByDrug(string DrugId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(dp => dp.DrugId == DrugId).ToListAsync();

        public async Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugsByHealthPlan(string HealthPlanId) => await _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(dp => dp.NHISHealthPlanId == HealthPlanId).ToListAsync();

        public PagedList<NHISHealthPlanDrugDtoForView> GetHealthPlanDrugsByHealthPlan(string HealthPlanId, PaginationParameter paginationParameter)
        {
            var HMOHealthPlanDrugPrices = _applicationDbContext.NHISHealthPlanDrugs.Include(dp => dp.Drug).Include(dp => dp.NHISHealthPlan).Where(dp => dp.NHISHealthPlanId == HealthPlanId).OrderBy(h => h.Drug.Name).ToList();
            var HMOHealthPlanDrugPricesToReturn = _mapper.Map<IEnumerable<NHISHealthPlanDrugDtoForView>>(HMOHealthPlanDrugPrices);
            return PagedList<NHISHealthPlanDrugDtoForView>.ToPagedList(HMOHealthPlanDrugPricesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        
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
