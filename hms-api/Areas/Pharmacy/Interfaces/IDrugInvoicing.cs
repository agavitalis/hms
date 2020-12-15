using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugInvoicing
    {
        Task<bool> CheckIfDrugsExist(List<Drugs> drugs);
        Task<string> GenerateDrugDispenseInvoice(DrugInvoicingDto drugInvoicingDto);
        Task<object> CreateDespenseRequest(DrugInvoicingDto drugInvoicingDto, string invoiceId);
        //Task<bool> CreateDrugPrescription(DrugPrescriptionInvoiceDtoForCreate drugPrescription, string invoiceId);
        //Task<IEnumerable<DrugDspensingInvoice>> GetDrugPrescriptionInvoices();
        //Task<IEnumerable<dynamic>> GetDrugPrescriptionsByInvoice(string invoiceId);
        //Task<IEnumerable<DrugPrescriptionInvoiceDtoForView>> GetDrugPrescriptionInvoicesForPatient(string patientId);
        //Task<DrugDispensing> GetDrugPrescription(string drugPrescriptionId);
        //Task<bool> CheckIfDrugPrescriptionIdExists(List<string> drugPrescriptionIds);
        //Task<bool> CheckIfAmountPaidIsCorrect(DrugPrescriptionPaymentDto drugPrescription);
        //Task<bool> PayForDrugs(DrugPrescriptionPaymentDto drugPrescription);
    }
}
