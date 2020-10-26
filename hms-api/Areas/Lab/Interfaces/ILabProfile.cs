using HMS.Areas.Lab.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabProfile
    {

        Task<object> GetLabByIdAsync(string LabId);
        Task<object> GetAllLabAsync();
        Task<bool> EditLabProfileAsync(EditLabProfileViewModel LabProfile);
        Task<bool> EditLabProfilePictureAsync(LabProfilePictureViewModel LabProfile);

    }
}
