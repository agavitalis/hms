using HMS.Areas.Admissions.Dtos;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IWardPersonnel
    {
        PagedList<WardPersonnelDtoForView> GetWardPersonnels(PaginationParameter paginationParameter);
        Task<object> GetWardPersonnel(string GetWardPersonnelId);
        Task<bool> UpdateBasicInfo(WardPersonnelBasicInfoDtoForUpdate WardPersonnel);
        Task<bool> UpdateContactDetails(WardPersonnelContactDetailsDtoForUpdate WardPersonnel);
        Task<bool> UpdateProfilePictureAsync(WardPersonnelProfilePictureDtoForUpdate WardPersonnel);
    }
}
