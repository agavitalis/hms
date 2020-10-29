using HMS.Areas.Accountant.ViewModels;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Interfaces
{
    public interface IAccountProfile
    {
        Task<object> GetAccountantByIdAsync(string AccountantId);
        Task<bool> EditAccountProfileAsync(EditAccountProfileViewModel AccountProfile);
        Task<bool> EditAccountProfilePictureAsync(AccountProfilePictureViewModel AccountProfile);
      
    }
}
