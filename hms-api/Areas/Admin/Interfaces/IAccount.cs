using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAccount
    {
        Task<Account> GetAccountByIdAsync(string id);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<bool> CreateAccount(Account account);
        Task<bool> UpdateAccount(Account account);
        Task<bool> DeleteAccount(Account account);
    }
}
