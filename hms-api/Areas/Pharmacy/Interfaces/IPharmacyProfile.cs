using HMS.Areas.Lab.ViewModels;
using HMS.Areas.Pharmacy.ViewModels;
using System.Threading.Tasks;


namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IPharmacyProfile
    {
        Task<object> GetPharmacyProfileByIdAsync(string PharmacyId);
        Task<object> GetAllPharmacyAsync();
        Task<bool> EditPharmacistBasicInfoAsync(EditPharmacistBasicInfoViewModel pharmacist);
        Task<bool> EditPharmacistContactDetailsAsync(EditPharmacistContactDetailsViewModel pharmacist);
        Task<bool> EditPharmacyProfilePictureAsync(PharmacyProfilePictureViewModel PharmacyProfile);

    }
}