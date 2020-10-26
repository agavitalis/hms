﻿using HMS.Areas.Pharmacy.ViewModels;
using System.Threading.Tasks;


namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IPharmacyProfile
    {
        Task<object> GetPharmacyProfileByIdAsync(string PharmacyId);
        Task<object> GetAllPharmacyAsync();
        Task<bool> EditPharmacyProfileAsync(EditPharmacyProfileViewModel PharmacyProfile);
        Task<bool> EditPharmacyProfilePictureAsync(PharmacyProfilePictureViewModel PharmacyProfile);

    }
}