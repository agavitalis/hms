using HMS.Areas.Accountant.Interfaces;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Repositories
{
    public class AccountantInvoiceRepository : IAccountantInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountantInvoiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        

        

        
       
    }
}
