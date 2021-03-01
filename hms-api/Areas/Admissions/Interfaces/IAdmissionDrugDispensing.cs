using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionDrugDispensing
    {
        Task<bool> CreateDrugDispensing(AdmissionDrugDispensing AdmissionRequest);
        Task<bool> UpdateDrugDispensing(AdmissionDrugDispensingDtoForCreate AdmissionRequest, AdmissionInvoice admissionInvoice);
    }
}
