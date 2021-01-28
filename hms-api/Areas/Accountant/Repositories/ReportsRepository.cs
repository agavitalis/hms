using HMS.Areas.Accountant.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Repositories
{
    public class ReportsRepository : IReports
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ReportsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }
        
        public async Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate).OrderBy(t => t.InvoiceType).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.PaymentMethod == PaymentMethod).OrderBy(t => t.InvoiceType).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Drug").ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Drug" && t.PaymentMethod == PaymentMethod).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Service Request").ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Service Request" && t.PaymentMethod == PaymentMethod).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Registration").ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Registration" && t.PaymentMethod == PaymentMethod).ToListAsync();
       

    }
}
