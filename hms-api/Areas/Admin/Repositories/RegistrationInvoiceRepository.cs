using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class RegistrationInvoiceRepository: IRegistrationInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
      
        public RegistrationInvoiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
           
        }

        public async Task<int> GetPaidRegistrationInvoicesCount()
        {
            var paidRegInvoice = await _applicationDbContext.RegistrationInvoices.Where(a => a.PaymentStatus == "Not Paid").CountAsync();
            return paidRegInvoice;
        }

        public async Task<int> GetUnPaidRegistrationInvoicesCount()
        {
            var unPaidRegInvoice = await _applicationDbContext.RegistrationInvoices.Where(a => a.PaymentStatus == "Paid").CountAsync();
            return unPaidRegInvoice;
        }

    }
}
