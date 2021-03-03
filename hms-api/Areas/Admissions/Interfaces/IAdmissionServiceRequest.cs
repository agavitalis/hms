using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionServiceRequest
    {
        Task<AdmissionServiceRequest> GetServiceRequest(string serviceRequestId);
        Task<bool> CreateAdmissionRequest(AdmissionServiceRequest AdmissionRequest);
        Task<bool> UpdateAdmissionServiceRequest(AdmissionServiceRequestDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice);
        PagedList<AdmissionServiceRequestDtoForView> GetAdmissionServiceRequests(string InvoiceId, PaginationParameter paginationParameter);
        Task<bool> CheckIfServiceRequestIdExist(List<string> serviceRequestIds);
        Task<bool> CheckIfAmountPaidIsCorrect(AdmissionServiceRequestPaymentDto serviceRequest);
    }
}
