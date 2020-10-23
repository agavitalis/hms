using HMS.ViewModels.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Lab
{
    public interface ILabProfile
    {

        Task<object> GetLabByIdAsync(string LabId);
        Task<object> GetAllLabAsync();
        Task<bool> EditLabProfileAsync(EditLabProfileViewModel LabProfile);
        Task<bool> EditLabProfilePictureAsync(LabProfilePictureViewModel LabProfile);

    }
}
