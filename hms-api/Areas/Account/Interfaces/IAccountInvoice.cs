using System.Threading.Tasks;

namespace HMS.Areas.Account.Interfaces
{
    public interface IAccountInvoice
    {
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabForPatientAsync(string PatientId);
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyForPatientAsync(string PatientId);
        
    }
}
