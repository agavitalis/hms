using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionDrugDispensing
    {
        Task<bool> CreateDrugDispensing(AdmissionDrugDispensing AdmissionRequest);
        Task<bool> UpdateDrugDispensing(MedicationDtoForAdminister AdmissionRequest, AdmissionInvoice admissionInvoice);
        Task<IEnumerable<dynamic>> GetDrugsInAnInvoice(string InvoiceId);

        PagedList<AdmissionDrugDispensingDtoForView> GetAdmissionDrugDispensing(string InvoiceId, PaginationParameter paginationParameter);
        Task<bool> CheckIfAmountPaidIsCorrect(string invoiceId, decimal amount);
        Task<bool> PayForDrugs(DrugDispensingPaymentDto drugInvoice);
        Task<bool> PayForDrugsWithAccount(DrugDispensingPaymentDto drugInvoice);
    }
}
