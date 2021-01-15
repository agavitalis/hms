using HMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface ITransactionLog
    {
        Task<bool> LogTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate, string BenefactorId, string InitiatorId);
        bool LogTransactionNotAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate, string BenefactorId, string InitiatorId);
        Task<IEnumerable<dynamic>> GetAccountTransactions(string AccountId);
    }
}
