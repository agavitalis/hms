﻿using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IServices
    {
        Task<IEnumerable<ServiceCategoryDtoForView>> GetAllServiceCategories();
        Task<ServiceCategory> GetServiceCategoryByIdAsync(string serviceCategoryId);
        Task<bool> CreateServiceCategoryAsync(ServiceCategory serviceCategory);
        Task<bool> UpdateServiceCategory(ServiceCategory serviceToEdit);
        Task<bool> DeleteServiceCategory(ServiceCategory serviceCategory);
        Task<IEnumerable<ServiceDtoForView>> GetAllServicesInAServiceCategory(string serviceCategoryId);



        Task<IEnumerable<ServiceDtoForView>> GetAllServices();
        Task<Service> GetServiceByIdAsync(string id);
        Task<bool> CreateService(Service serviceDtoForCreate);
        Task<bool> UpdateService(Service serviceToEdit);
        Task<bool> DeleteService(Service service);
        Task<bool> CreateServiceRequest(ServiceRequestDtoForCreate serviceRequest, string invoiceId);
        Task<bool> CheckIfServicesExist(List<string> serviceIds);
        Task<string> GenerateInvoiceForServiceRequest(ServiceRequestDtoForCreate serviceRequest);

      
        Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoices(PaginationParameter paginationParameter);

        Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoices();
        Task<IEnumerable<ServiceRequestForView>> GetServiceRequestInAnInvoice(string invoiceId);      
        Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoiceForPatient(string patientId, PaginationParameter paginationParameter);
        Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoiceForPatient(string patientId);

        Task<ServiceRequest> GetServiceRequest(string serviceRequestId);
        Task<bool> CheckIfServiceRequestIdExist(List<string> serviceRequestIds);
        Task<bool> CheckIfAmountPaidIsCorrect(ServiceRequestPaymentDto serviceRequest);
        Task<bool> PayForServices(ServiceRequestPaymentDto serviceRequest);
        Task<ServiceRequestResult> UploadServiceRequestResult(ServiceRequestResult serviceRequestResult);
        Task<bool> UploadServiceRequestResultImage(ServiceUploadResultDto serviceRequestResultImage, string serviceRequestResultId);
        Task<IEnumerable<ServiceRequestResult>> GetServiceRequestResults(string serviceRequestId);
    }
}
