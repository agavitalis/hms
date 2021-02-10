using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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
        private readonly IMapper _mapper;

        public HealthPlanRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HealthPlan>> GetAllHealthPlan() => await _applicationDbContext.HealthPlans.Where(h => h.Status == true).OrderBy(h => h.Name).ToListAsync();

        public async Task<HealthPlan> GetHealthPlanByIdAsync(string id)
        {
            try
            {
               
                var plan =await  _applicationDbContext.HealthPlans.Where(h => h.Id == id && h.Status == true).FirstAsync();

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

        public async Task<bool> UpdateHealthPlan(HealthPlan healthPlan)
        {
            try
            {
                if (healthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.HealthPlans.Update(healthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteHealthPlan(HealthPlan healthPlan)
        {
            try
            {
                if (healthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.HealthPlans.Remove(healthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<HealthPlanDtoForView> GetHealthPlansPagination(PaginationParameter paginationParameter)
        {
            var healthPlans = _applicationDbContext.HealthPlans.Where(h => h.Status == true).OrderBy(h => h.Name).ToList(); 
            var healthPlansToReturn = _mapper.Map<IEnumerable<HealthPlanDtoForView>>(healthPlans);
            return PagedList<HealthPlanDtoForView>.ToPagedList(healthPlansToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }



   
    }
}
