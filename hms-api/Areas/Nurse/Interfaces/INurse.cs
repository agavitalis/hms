using HMS.Areas.Nurse.Dtos;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Interfaces.Nurse
{
    public interface INurse
    {
        PagedList<NurseDtoForView> GetNurses(PaginationParameter paginationParameter);
        Task<object> GetNurse(string NurseId);
        Task<bool> EditBasicInfo(NurseBasicInfoDtoForEdit AccountProfile);
        Task<bool> EditContactDetails(NurseContactDetailsDtoForEdit AccountProfile);
        Task<bool> EditProfilePictureAsync(NurseProfilePictureDtoForEdit AccountProfile);
    }
}
