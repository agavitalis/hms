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

        public async Task<bool> CreateTransaction(Transactions transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return false;
                }

                _applicationDbContext.Transactions.Add(transaction);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
