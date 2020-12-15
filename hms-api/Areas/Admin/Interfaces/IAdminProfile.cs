using HMS.Areas.Admin.ViewModels;
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
        Task<bool> EditAdminBasicInfo(EditAdminBasicInfoViewModel AdminProfile);
        Task<bool> EditAdminContactDetails(EditAdminContactDetailsViewModel AdminProfile);
        Task<bool> EditAdminProfilePictureAsync(AdminProfilePictureViewModel AdminProfile);
    }
}
