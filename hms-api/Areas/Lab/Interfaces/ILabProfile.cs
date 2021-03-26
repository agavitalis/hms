using HMS.Areas.Lab.Dtos;
using HMS.Areas.Lab.ViewModels;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabProfile
    {

        Task<LabAttendantDtoForView> GetLabAttendant(string LabId);
        PagedList<LabAttendantDtoForView> GetLabAttendants(PaginationParameter paginationParameter);
        Task<object> GetLabProfiles();
        Task<bool> EditLabProfileBasicInfoAsync(EditLabProfileBasicInfoViewModel LabProfile);
        Task<bool> EditLabProfileContactDetailsAsync(EditLabProfileContactDetailsViewModel LabProfile);
        Task<bool> EditLabProfilePictureAsync(LabProfilePictureViewModel LabProfile);

    }
}
