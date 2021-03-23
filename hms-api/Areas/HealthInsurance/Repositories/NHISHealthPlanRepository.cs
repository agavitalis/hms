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
    public class NHISHealthPlanRepository : INHISHealthPlan
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public NHISHealthPlanRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateNHISHealthPlan(NHISHealthPlan HealthPlan)
        {
            try
            {
                if (HealthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlans.Add(HealthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NHISHealthPlan> GetNHISHealthPlan(string HMOId) => await _applicationDbContext.NHISHealthPlans.Where(h => h.Id == HMOId).Include(h => h.HealthPlan).FirstOrDefaultAsync();
    
        public PagedList<NHISHealthPlanDtoForView> GetNHISHealthPlans(PaginationParameter paginationParameter)
        {
            var NHISHealthPlans = _applicationDbContext.NHISHealthPlans.Include(h => h.HealthPlan).ToList();
            var HMOsToReturn = _mapper.Map<IEnumerable<NHISHealthPlanDtoForView>>(NHISHealthPlans);
            return PagedList<NHISHealthPlanDtoForView>.ToPagedList(HMOsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> UpdateNHISHealthPlan(NHISHealthPlan HealthPlan)
        {
            try
            {
                if (HealthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlans.Update(HealthPlan);
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
