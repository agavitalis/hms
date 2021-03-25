using AutoMapper;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.NHIS.Dtos;
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
    public class HMOUserGroupRepository : IHMOUserGroup
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public HMOUserGroupRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateHMOUserGroup(HMOUserGroup HMOUserGroup)
        {
            try
            {
                if (HMOUserGroup == null)
                {
                    return false;
                }

                _applicationDbContext.HMOUserGroups.Add(HMOUserGroup);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOUserGroup> GetHMOUserGroup(string HMOUserGroupId) => await _applicationDbContext.HMOUserGroups.Where(h => h.Id == HMOUserGroupId).Include(h => h.HMO).ThenInclude(h => h.HealthPlan).FirstOrDefaultAsync();
       

        public PagedList<HMOUserGroupDtoForView> GetHMOUserGroups(PaginationParameter paginationParameter, string HMOId)
        {
            var HMOUserGroups = _applicationDbContext.HMOUserGroups.Include(h => h.HMO).ThenInclude(h => h.HealthPlan).Where(h => h.HMOId == HMOId).ToList();
            var HMOUserGroupsToReturn = _mapper.Map<IEnumerable<HMOUserGroupDtoForView>>(HMOUserGroups);
            return PagedList<HMOUserGroupDtoForView>.ToPagedList(HMOUserGroupsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<int> GetUserGroupCount(string HMOId) => await _applicationDbContext.HMOUserGroups.Where(h => h.HMOId == HMOId).CountAsync();


        public async Task<bool> UpdateHMOUserGroup(HMOUserGroup HMOUserGroup)
        {
            try
            {
                if (HMOUserGroup == null)
                {
                    return false;
                }

                _applicationDbContext.HMOUserGroups.Update(HMOUserGroup);
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
