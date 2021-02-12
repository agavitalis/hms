using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionRequest
    {
        Task<bool> CreateAdmissionRequest(AdmissionRequest AdmissionRequest);
        Task<bool> UpdateAdmissionRequest(AdmissionRequestDtoForCreate AdmissionRequest, AdmissionInvoice admissionInvoice);

    }
}
