using HMS.Areas.Accountant.ViewModels;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Interfaces
{
    public interface IAccountantProfile
    {
        Task<object> GetAccountant(string AccountantId);
        Task<object> GetAccountants();
        Task<bool> EditAccountantBasicInfo(EditAccountantBasicInfoViewModel AccountProfile);
        Task<bool> EditAccountantContactDetails(EditAccountantContactDetailsViewModel AccountProfile);
        Task<bool> EditAccountProfilePictureAsync(AccountProfilePictureViewModel AccountProfile);
      
    }
}
