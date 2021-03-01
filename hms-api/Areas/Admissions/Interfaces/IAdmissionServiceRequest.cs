using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionServiceRequest
    {
        Task<bool> CreateAdmissionRequest(AdmissionServiceRequest AdmissionRequest);
        Task<bool> UpdateAdmissionServiceRequest(AdmissionServiceRequestDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice);
    }
}
