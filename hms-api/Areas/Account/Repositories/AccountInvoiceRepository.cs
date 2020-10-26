using HMS.Areas.Account.Interfaces;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Account.Repositories
{
    public class AccountInvoiceRepository : IAccountInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountInvoiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<object> GetUnpaidFeeInvoiceGeneratedByLabAsync()
        {
            var UnpaidLabReciepts = await _applicationDbContext.Invoices.Where(a => a.PaymentSource == "Lab" && a.PaymentStatus == false)
                        .Join(
                              _applicationDbContext.ApplicationUsers,
                              FeeInvoice => FeeInvoice.PatientId,
                              applicationUser => applicationUser.Id,
                            (FeeInvoice, applicationUser) => new { FeeInvoice, applicationUser }

                        )
                        .Join(
                          _applicationDbContext.PatientProfiles,
                          applicationUser => applicationUser.applicationUser.Id,
                          PatientProfiles => PatientProfiles.PatientId,
                          (applicationUser, PatientProfiles) => new { applicationUser, PatientProfiles }
                        )
                        .ToListAsync();
                      

                       
            return UnpaidLabReciepts;

                   
            
            
        }

        public async Task<object> GetUnpaidFeeInvoiceGeneratedByLabForPatientAsync(string PatientId)
        {
            var UnpaidLabReciepts = await _applicationDbContext.Invoices.Where(a => a.PaymentSource == "Lab" && a.PaymentStatus == false && a.PatientId == PatientId)
                       .Join(
                             _applicationDbContext.ApplicationUsers,
                             FeeInvoice => FeeInvoice.PatientId,
                             applicationUser => applicationUser.Id,
                           (FeeInvoice, applicationUser) => new { FeeInvoice, applicationUser }

                       )
                       .Join(
                         _applicationDbContext.PatientProfiles,
                         applicationUser => applicationUser.applicationUser.Id,
                         PatientProfiles => PatientProfiles.PatientId,
                         (applicationUser, PatientProfiles) => new { applicationUser, PatientProfiles }
                       )
                       .ToListAsync();



            return UnpaidLabReciepts;
        }

        public async Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyAsync()
        {
            var UnpaidPharmacyReciepts = await _applicationDbContext.Invoices.Where(a => a.PaymentSource == "Pharmacy" && a.PaymentStatus == false)
                        .Join(
                              _applicationDbContext.ApplicationUsers,
                              FeeInvoice => FeeInvoice.PatientId,
                              applicationUser => applicationUser.Id,
                            (FeeInvoice, applicationUser) => new { FeeInvoice, applicationUser }

                        )
                        .Join(
                          _applicationDbContext.PatientProfiles,
                          applicationUser => applicationUser.applicationUser.Id,
                          PatientProfiles => PatientProfiles.PatientId,
                          (applicationUser, PatientProfiles) => new { applicationUser, PatientProfiles }
                        )
                        .ToListAsync();



            return  UnpaidPharmacyReciepts;




        }

        public async Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyForPatientAsync(string PatientId)
        {
            var UnpaidPharmacyReciepts = await _applicationDbContext.Invoices.Where(a => a.PaymentSource == "Pharmacy" && a.PaymentStatus == false && a.PatientId == PatientId)
                       .Join(
                             _applicationDbContext.ApplicationUsers,
                             FeeInvoice => FeeInvoice.PatientId,
                             applicationUser => applicationUser.Id,
                           (FeeInvoice, applicationUser) => new { FeeInvoice, applicationUser }

                       )
                       .Join(
                         _applicationDbContext.PatientProfiles,
                         applicationUser => applicationUser.applicationUser.Id,
                         PatientProfiles => PatientProfiles.PatientId,
                         (applicationUser, PatientProfiles) => new { applicationUser, PatientProfiles }
                       )
                       .ToListAsync();



            return UnpaidPharmacyReciepts;
        }
    }
}
