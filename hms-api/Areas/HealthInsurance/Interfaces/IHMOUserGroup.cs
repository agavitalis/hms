using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOUserGroup
    {
        Task<int> GetUserGroupCount(string HMOId);
        Task<HMOUserGroup> GetHMOUserGroup(string HMOUserGroupId);
        PagedList<HMOUserGroupDtoForView> GetHMOUserGroups(PaginationParameter paginationParameter, string HMOId);
        Task<bool> CreateHMOUserGroup(HMOUserGroup HMOUserGroup);
        Task<bool> UpdateHMOUserGroup(HMOUserGroup HMOUserGroup);
    }
}
