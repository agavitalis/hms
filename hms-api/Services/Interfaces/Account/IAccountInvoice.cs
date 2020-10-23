using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Account
{
    public interface IAccountInvoice
    {
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabForPatientAsync(string PatientId);
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyForPatientAsync(string PatientId);
        
    }
}
