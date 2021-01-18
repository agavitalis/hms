using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class TransactionLogRepository : ITransactionLog
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TransactionLogRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<dynamic>> GetAccountTransactions(string AccountId) => await _applicationDbContext.Transactions
            .Where(t => t.BenefactorId == AccountId).Include(t => t.Benefactor)
            .Select(p => new
            {
                Id = p.Id,
                Amount = p.Amount,
                TransactionType = p.TransactionType,
                Description = p.Description,
                TrasactionDate = p.TrasactionDate,
                AccountBalance = p.Benefactor.AccountBalance,
                PaidBy = p.Initiator.FirstName + " " + p.Initiator.LastName

            })

            .ToListAsync();


        public async Task<bool> LogTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate, string BenefactorId, string InitiatorId)
        {
            try
            {
                if (amount != 0 && transactionType != null && description != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        Description = description,
                        TrasactionDate = transactionDate,
                        BenefactorId = BenefactorId,
                        InitiatorId = InitiatorId
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        public bool LogTransactionNotAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate, string BenefactorId, string InitiatorId)
            {
                try
                {
                    if (amount != 0 && transactionType != null && description != null && transactionDate != null)
                    {
                        var transaction = new Transactions()
                        {
                            Amount = amount,
                            TransactionType = transactionType,
                            InvoiceType = invoiceType,
                            InvoiceId = invoiceId,
                            Description = description,
                            TrasactionDate = transactionDate,
                            BenefactorId = BenefactorId,
                            InitiatorId = InitiatorId
                        };

                        _applicationDbContext.Transactions.Add(transaction);
                         _applicationDbContext.SaveChanges();

                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
         }
    }

    
}

