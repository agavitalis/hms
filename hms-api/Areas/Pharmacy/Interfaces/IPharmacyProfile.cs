using HMS.Areas.Lab.ViewModels;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Services.Helpers;
using System.Threading.Tasks;


namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IPharmacyProfile
    {
        Task<object> GetPharmacyProfileByIdAsync(string PharmacyId);
        Task<object> GetAllPharmacyAsync();
        PagedList<PharmacistDtoForView> GetPharmacists(PaginationParameter paginationParameter);
        Task<PharmacistDtoForView> GetPharmacist(string PharmacistId);
        Task<bool> EditPharmacistBasicInfoAsync(EditPharmacistBasicInfoViewModel pharmacist);
        Task<bool> EditPharmacistContactDetailsAsync(EditPharmacistContactDetailsViewModel pharmacist);
        Task<bool> EditPharmacyProfilePictureAsync(PharmacyProfilePictureViewModel PharmacyProfile);

    }
}