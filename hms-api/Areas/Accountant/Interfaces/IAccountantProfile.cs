using HMS.Areas.Accountant.Dtos;
using HMS.Areas.Accountant.ViewModels;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Interfaces
{
    public interface IAccountantProfile
    {
        Task<AccountantDtoForView> GetAccountant(string AccountantId);
        PagedList<AccountantDtoForView> GetAccountants(PaginationParameter paginationParameter);
        Task<bool> EditAccountantBasicInfo(EditAccountantBasicInfoViewModel AccountProfile);
        Task<bool> EditAccountantContactDetails(EditAccountantContactDetailsViewModel AccountProfile);
        Task<bool> EditAccountProfilePictureAsync(AccountProfilePictureViewModel AccountProfile);
      
    }
}
