using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface ITransactionLog
    {
        Task<bool> CreateTransaction(Transactions transaction);
    }
}
