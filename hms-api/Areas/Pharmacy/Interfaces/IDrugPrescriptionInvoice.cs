using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugPrescriptionInvoice
    {
        Task<bool> CheckIfDrugsExist(List<string> serviceIds);
        Task<string> GenerateInvoiceForDrugPrescription(DrugPrescriptionInvoiceDtoForCreate drugPrescription);
        Task<bool> CreateDrugPrescription(DrugPrescriptionInvoiceDtoForCreate drugPrescription, string invoiceId);
        Task<IEnumerable<DrugPrescriptionInvoice>> GetDrugPrescriptionInvoices();
        Task<IEnumerable<dynamic>> GetDrugPrescriptionsByInvoice(string invoiceId);
        Task<IEnumerable<DrugPrescriptionInvoiceDtoForView>> GetDrugPrescriptionInvoicesForPatient(string patientId);
        Task<DrugPrescription> GetDrugPrescription(string drugPrescriptionId);
        Task<bool> CheckIfDrugPrescriptionIdExists(List<string> drugPrescriptionIds);
        Task<bool> CheckIfAmountPaidIsCorrect(DrugPrescriptionPaymentDto drugPrescription);
        Task<bool> PayForDrugs(DrugPrescriptionPaymentDto drugPrescription);
    }
}
