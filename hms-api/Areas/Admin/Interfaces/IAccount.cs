﻿using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAccount
    {
        Task<Account> GetAccountByIdAsync(string id);
        Task<AccountInvoice> GetAccountInvoice(string AccountInvoiceId);
        Task<Account> GetAccountByAccountNumber(string AccountNumber);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<AccountInvoice> CreateAccountInvoice(AccountInvoice accountInvoice);
        Task<bool> CreateAccount(Account account);
        Task<bool> UpdateAccount(Account account);
        Task<bool> DeleteAccount(Account account);

    }
}
