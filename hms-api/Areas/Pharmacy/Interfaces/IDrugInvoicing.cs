﻿using HMS.Areas.Pharmacy.Dtos;
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
        Task<IEnumerable<DrugDispensingInvoice>> GetDrugDispencingInvoices();
        Task<DrugDispensingInvoice> GetDrugDispencingInvoice(string DrugInvoiceId);
        Task<IEnumerable<DrugDispensingInvoice>> GetPatientDrugInvoices(string patientId);
        Task<IEnumerable<dynamic>> GetDrugsInAnInvoice(string invoiceNumber);
        Task<bool> CheckIfAmountPaidIsCorrect(string invoiceNumber, decimal amount);
        Task<bool> PayForDrugs(DrugInvoicingPaymentDto drugInvoice);
        Task<bool> PayForDrugsWithAccount(DrugInvoicingPaymentDto drugInvoice);
        Task<bool> UpdateDrugInvoice(DrugDispensingInvoice invoice);
        Task<int> GetPaidDrugInvoiceCount();
        Task<int> GetUnPaidDrugInvoiceCount();
        Task<int> GetPaidDrugInvoiceDispensedCount();
        Task<int> GetPaidDrugInvoiceNotDispensedCount();
    }
}

