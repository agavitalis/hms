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
        PagedList<AdmissionServiceRequestDtoForView> GetAdmissionServiceRequests(string InvoiceId, PaginationParameter paginationParameter);
        Task<AdmissionServiceRequestResult> UploadServiceRequestResult(AdmissionServiceRequestResult serviceRequestResult);
        Task<bool> UploadServiceRequestResultImage(AdmissionServiceUploadResultDto serviceRequestResultImage, string serviceRequestResultId);
        PagedList<AdmissionServiceRequestResultDtoForView> GetServiceRequestResultsPagination(string serviceRequestId, PaginationParameter paginationParameter);
    }
}
