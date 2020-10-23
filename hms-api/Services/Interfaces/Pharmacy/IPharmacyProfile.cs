using System.Threading.Tasks;
using HMS.ViewModels.Pharmacy;

namespace HMS.Services.Interfaces.Pharmacy
{
    public interface IPharmacyProfile
    {
        Task<object> GetPharmacyProfileByIdAsync(string PharmacyId);
        Task<object> GetAllPharmacyAsync();
        Task<bool> EditPharmacyProfileAsync(EditPharmacyProfileViewModel PharmacyProfile);
        Task<bool> EditPharmacyProfilePictureAsync(PharmacyProfilePictureViewModel PharmacyProfile);

    }
}