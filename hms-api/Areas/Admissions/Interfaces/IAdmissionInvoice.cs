using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionInvoice
    {
        Task<AdmissionInvoice> GetAdmissionInvoice(string AdmissionInvoiceId);
        Task<AdmissionInvoice> GetAdmissionInvoiceByAdmissionId(string AdmissionId);
        Task<string> CreateAdmissionInvoice(AdmissionInvoice AdmissionInvoice);
        Task<bool> UpdateAdmissionInvoice(AdmissionInvoice AdmissionInvoice);
        Task<string> UpdateAdmissionInvoice(ServiceMedicationDtoForAdminister AdmissionRequest, AdmissionInvoice AdmissionInvoice);
        Task<string> UpdateAdmissionInvoice(DrugMedicationDtoForAdminister AdmissionRequest, AdmissionInvoice AdmissionInvoice);
        Task<bool> CheckIfAmountPaidIsCorrect(AdmissionPaymentDto admissionPayment);
        Task<bool> PayForAdmission(AdmissionPaymentDto admissionPayment);
        Task<bool> PayForAdmissionWithAccount(AdmissionPaymentDto admissionPayment);

    }
}
