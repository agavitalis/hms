using AutoMapper;
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
    public class HMOSubUserGroupRepository : IHMOSubUserGroup
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public HMOSubUserGroupRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateHMOUserGroup(HMOSubUserGroup HMOSubUserGroup)
        {
            try
            {
                if (HMOSubUserGroup == null)
                {
                    return false;
                }

                _applicationDbContext.HMOSubUserGroups.Add(HMOSubUserGroup);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOSubUserGroup> GetHMOSubUserGroup(string HMOSubUserGroupId) => await _applicationDbContext.HMOSubUserGroups.Where(h => h.Id == HMOSubUserGroupId)
                                                                                            .Include(h => h.HMOUserGroup).ThenInclude(h => h.HMO)
                                                                                            .ThenInclude(h => h.HealthPlan).FirstOrDefaultAsync();
        

        public PagedList<HMOSubUserGroupDtoForView> GetHMOSubUserGroups(PaginationParameter paginationParameter, string HMOUserGroupId)
        {
            var HMOSubUserGroups = _applicationDbContext.HMOSubUserGroups.Include(h => h.HMOUserGroup).ThenInclude(h => h.HMO).ThenInclude(h => h.HealthPlan).Where(h => h.HMOUserGroupId == HMOUserGroupId).ToList();
            var HMOSubUserGroupsToReturn = _mapper.Map<IEnumerable<HMOSubUserGroupDtoForView>>(HMOSubUserGroups);
            return PagedList<HMOSubUserGroupDtoForView>.ToPagedList(HMOSubUserGroupsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
