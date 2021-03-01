using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionInvoice
    {
        Task<AdmissionInvoice> GetAdmissionInvoice(string AdmissionInvoiceId);
        Task<AdmissionInvoice> GetAdmissionInvoiceByAdmissionId(string AdmissionId);
        Task<string> CreateAdmissionInvoice(AdmissionInvoice AdmissionInvoice);
        Task<string> UpdateAdmissionInvoice(AdmissionServiceRequestDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice);
        Task<string> UpdateAdmissionInvoice(AdmissionDrugDispensingDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice);
    }
}
