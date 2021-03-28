using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOSubUserGroup
    {
        Task<int> GetSubUserGroupCount(string UserGroupId);
        Task<HMOSubUserGroup> GetHMOSubUserGroup(string HMOSubUserGroupId);
        PagedList<HMOSubUserGroupDtoForView> GetHMOSubUserGroups(PaginationParameter paginationParameter, string HMOUserGroupId);
        Task<bool> CreateHMOUserGroup(HMOSubUserGroup HMOSubUserGroup);
        Task<bool> UpdateHMOUserGroup(HMOSubUserGroup HMOSubUserGroup);
    }
}
