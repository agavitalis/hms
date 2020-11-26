using HMS.Areas.Lab.ViewModels;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabProfile
    {

        Task<LabProfile> GetLabByIdAsync(string LabId);
        Task<object> GetLabProfiles();
        Task<bool> EditLabProfileBasicInfoAsync(EditLabProfileBasicInfoViewModel LabProfile);
        Task<bool> EditLabProfileContactDetailsAsync(EditLabProfileContactDetailsViewModel LabProfile);
        Task<bool> EditLabProfilePictureAsync(LabProfilePictureViewModel LabProfile);

    }
}
