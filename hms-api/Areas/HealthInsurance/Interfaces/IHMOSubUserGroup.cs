using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOSubUserGroup
    {
        Task<HMOSubUserGroup> GetHMOSubUserGroup(string HMOSubUserGroupId);
        PagedList<HMOSubUserGroupDtoForView> GetHMOSubUserGroups(PaginationParameter paginationParameter);
        Task<bool> CreateHMOUserGroup(HMOSubUserGroup HMOSubUserGroup);
    }
}
