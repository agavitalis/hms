using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Interfaces
{
    public interface IReports
    {
        Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate, string PaymentMethod);
        Task<IEnumerable<Transactions>> GetTransactionsForAccounts(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transactions>> GetTransactionsForAccounts(DateTime startDate, DateTime endDate, string TransactionType);
        Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate, string PaymentMethod);
        Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate, string PaymentMethod);
        Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate, string PaymentMethod);
    }
}
