using HMS.Areas.Admin.Dtos;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IRegister
    { 
        Task<File> CreateFile(string AccountId);
        Task<string> RegisterPatient(ApplicationUser patient, File file, Account account);
        Task<object> GetPatientRegistrationInvoice(string patientId);
        Task<RegistrationInvoice> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy, string patientId);
        Task<int> PayRegistrationFee(PatientRegistrationPaymentDto paymentDetails);
        Task<int> PayRegistrationFeeWithAccount(PatientRegistrationPaymentDto paymentDetails);
        Task<RegistrationInvoice> GetRegistrationInvoice(string PatientId);
        Task<IEnumerable<RegistrationInvoice>> GetRegistrationInvoices();
    }
}
