using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAdminProfile
    {
        Task<object> GetAdmin(string AdminId);
        Task<object> GetAdmins();
        PagedList<AdminProfileDtoForView> GetAdminsPagnation(PaginationParameter paginationParameter);
        Task<bool> EditAdminBasicInfo(EditAdminBasicInfoViewModel AdminProfile);
        Task<bool> EditAdminContactDetails(EditAdminContactDetailsViewModel AdminProfile);
        Task<bool> EditAdminProfilePictureAsync(AdminProfilePictureViewModel AdminProfile);
    }
}
