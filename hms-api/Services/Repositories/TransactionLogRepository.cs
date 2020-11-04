using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
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

        public async Task<bool> LogTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string description, DateTime transactionDate)
        {
            try
            {
                if (amount != 0 && transactionType != null && invoiceType != null && invoiceId != null && description != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        Description = description,
                        TrasactionDate = transactionDate
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
    }
}

