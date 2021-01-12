using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ServiceRequestInvoiceRepository: IServiceRequestInvoice
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public ServiceRequestInvoiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }

        public async Task<int> GetPaidServiceRequestInvoiceCount()
        {

            var paidServiceRequestInvoiceCount = await _applicationDbContext.ServiceInvoices.Where(i => i.PaymentStatus == "Paid").CountAsync();
            return paidServiceRequestInvoiceCount;
        }

        public async Task<int> GetUnPaidServiceRequestInvoiceCount()
        {

            var unPaidServiceRequestInvoiceCount = await _applicationDbContext.ServiceInvoices.Where(i => i.PaymentStatus == "Not Paid").CountAsync();
            return unPaidServiceRequestInvoiceCount;
        }
    }
}
