﻿using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    public class HMOHealthPlanRepository : IHMOHealthPlan
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public HMOHealthPlanRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateHMOHealthPlan(HMOHealthPlan HMOHealthPlan)
        {
            try
            {
                if (HMOHealthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlans.Add(HMOHealthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOHealthPlan> GetHMOHealthPlan(string HMOId) => await _applicationDbContext.HMOHealthPlans.Where(h => h.Id == HMOId).Include(h => h.HMO).FirstOrDefaultAsync();


        public PagedList<HMOHealthPlanDtoForView> GetHMOHealthPlans(PaginationParameter paginationParameter)
        {
            var HMOs = _applicationDbContext.HMOHealthPlans.Include(h => h.HMO).ToList();
            var HMOHealthPlansToReturn = _mapper.Map<IEnumerable<HMOHealthPlanDtoForView>>(HMOs);
            return PagedList<HMOHealthPlanDtoForView>.ToPagedList(HMOHealthPlansToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
