using HMS.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Account
{
    public interface IAccountProfile
    {
        Task<object> GetAccountantByIdAsync(string AccountantId);
        Task<bool> EditAccountProfileAsync(EditAccountProfileViewModel AccountProfile);
        Task<bool> EditAccountProfilePictureAsync(AccountProfilePictureViewModel AccountProfile);
      
    }
}
