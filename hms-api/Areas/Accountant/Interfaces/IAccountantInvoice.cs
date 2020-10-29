using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Interfaces
{
    public interface IAccountantInvoice
    {
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByLabForPatientAsync(string PatientId);
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyAsync();
        Task<object> GetUnpaidFeeInvoiceGeneratedByPharmacyForPatientAsync(string PatientId);
        
    }
}
