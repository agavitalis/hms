using HMS.Models;
using HMS.Services.Dtos;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface ITransactionLog
    {
        Task<bool> LogTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorId, string InitiatorId);
        bool LogAccountTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAccountId, string InitiatorId);
        Task<bool> LogAccountTransactionAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAccountId, string InitiatorId);

        Task<bool> LogLinkPaymentTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate, string BenefactorId, string DepositorsName);
        bool LogTransactionNotAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorId, string InitiatorId);
        Task<IEnumerable<dynamic>> GetAccountTransactions(string AccountId);

        PagedList<TransactionsDtoForView> GetAccountTransactions(string AccountId, PaginationParameter paginationParameter);
    }
}
