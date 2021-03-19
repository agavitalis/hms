using HMS.Areas.HealthInsurance.Dtos;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOAdmin
    {
        PagedList<HMOAdminDtoForView> GetHMOAdmins(PaginationParameter paginationParameter);
        Task<object> GetHMOAdmin(string GetHMOAdminId);
        Task<bool> UpdateBasicInfo(HMOAdminBasicInfoDtoForUpdate HMOAdmin);
        Task<bool> UpdateContactDetails(HMOAdminContactDetailsDtoForUpdate HMOAdmin);
        Task<bool> UpdateProfilePictureAsync(HMOAdminProfilePictureDtoForUpdate HMOAdmin);
    }
}
