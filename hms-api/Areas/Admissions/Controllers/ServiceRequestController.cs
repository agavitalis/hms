using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Service Requests")]
    [ApiController]
    public class ServiceRequestController : Controller
    {
        private readonly IServices _serviceRepo;
        private readonly IMapper _mapper;
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmissionServiceRequest _admissionServiceRequest;
        private readonly IPatientProfile _patient;


        public ServiceRequestController(IServices serviceRepo, IMapper mapper, IAdmission admission, IAdmissionInvoice admissionInvoice, IAdmissionServiceRequest admissionServiceRequest, IPatientProfile patient)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
            _admission = admission;
            _admissionInvoice = admissionInvoice;
            _patient = patient;
            _admissionServiceRequest = admissionServiceRequest;
        }

        [Route("RequestServices")]
        [HttpPost]
        public async Task<IActionResult> RequestServices(AdmissionServiceRequestDtoForCreate AdmissionRequest)
        {
            if (AdmissionRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            //check if the admission exists
            var admission = await _admission.GetAdmission(AdmissionRequest.AdmissionId);
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(AdmissionRequest.AdmissionId);
            if (admission == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid Admission Id passed",
                });


            //check if all service id passed exist
            if (AdmissionRequest.ServiceId != null)
            {
                var servicesCheck = await _serviceRepo.CheckIfServicesExist(AdmissionRequest.ServiceId);
                if (!servicesCheck)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "One or more service id passed is/are invalid"
                    });

            }


            //update admission invoice price for request
            var invoiceId = await _admissionInvoice.UpdateAdmissionInvoice(AdmissionRequest, admissionInvoice);
            if (string.IsNullOrEmpty(invoiceId))
                return BadRequest(new
                {
                    response = "301",
                    message = "Failed to update invoice !!!, Try Again"
                });

            //insert request
            var result = await _admissionServiceRequest.UpdateAdmissionServiceRequest(AdmissionRequest, admissionInvoice);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Request Service Failed !!!, Try Again"
                });

            return Ok(new { message = "Admission Request submitted successfully" });
        }



        [HttpGet("GetServiceRequestsInAnInvoice")]
        public async Task<IActionResult> GetServiceRequestInAnInvoice(string AdmissionInvoiceId, [FromQuery] PaginationParameter paginationParameter)
        {
            var serviceRequests = _admissionServiceRequest.GetAdmissionServiceRequests(AdmissionInvoiceId, paginationParameter);

            var paginationDetails = new
            {
                serviceRequests.TotalCount,
                serviceRequests.PageSize,
                serviceRequests.CurrentPage,
                serviceRequests.TotalPages,
                serviceRequests.HasNext,
                serviceRequests.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                serviceRequests,
                paginationDetails,
                message = "Service Requests Fetched"
            });
        }



        [HttpGet("GetAdmissionServiceRequest/{AdmissionServiceRequestId}")]
        public async Task<IActionResult> GetAdmissionServiceRequest(string AdmissionServiceRequestId)
        {
            var admissionServiceRequest = await _admissionServiceRequest.GetServiceRequest(AdmissionServiceRequestId);
            if (admissionServiceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            return Ok(new
            {
                admissionServiceRequest,
                message = "Service Request"
            });
        }

        [HttpPost("UploadServiceRequestResult")]
        public async Task<IActionResult> UploadServiceRequestResult([FromForm] AdmissionServiceUploadResultDto serviceRequestResultForUpload)
        {
            if (serviceRequestResultForUpload == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceRequest = await _admissionServiceRequest.GetServiceRequest(serviceRequestResultForUpload.ServiceRequestId);

            if (serviceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceRequestResultToUpload = _mapper.Map<AdmissionServiceRequestResult>(serviceRequestResultForUpload);

            var serviceRequestResult = await _admissionServiceRequest.UploadServiceRequestResult(serviceRequestResultToUpload);
            
            if (serviceRequestResult == null)
            {
                return BadRequest(new { response = "301", message = "Service failed to create" });
            }

            if (serviceRequestResultForUpload.Images == null)
            {
                return Ok(new
                {
                    serviceRequestResult,
                    message = "Result Uploaded Successfully"
                });
            }
            var serviceRequestResultImage = await _admissionServiceRequest.UploadServiceRequestResultImage(serviceRequestResultForUpload, serviceRequestResult.Id);
            return Ok(new
            {
                serviceRequestResult,
                serviceRequestResultImage,
                message = "Result Uploaded Successfully"
            });
        }

       

        [HttpGet("GetServiceRequestResults/{ServiceRequestId}")]
        public async Task<IActionResult> GetServiceRequestResults(string ServiceRequestId, [FromQuery] PaginationParameter paginationParameter)
        {
            if (ServiceRequestId == null)
            {
                return BadRequest(new { message = "Service Request Id Not Passed" });
            }

            var serviceRequest = await _admissionServiceRequest.GetServiceRequest(ServiceRequestId);

            if (serviceRequest == null)
            {
                return BadRequest(new { response = "301", message = "Invalid Service Request Id" });
            }

            var serviceRequestResults = _admissionServiceRequest.GetServiceRequestResultsPagination(ServiceRequestId, paginationParameter);

            var paginationDetails = new
            {
                serviceRequestResults.TotalCount,
                serviceRequestResults.PageSize,
                serviceRequestResults.CurrentPage,
                serviceRequestResults.TotalPages,
                serviceRequestResults.HasNext,
                serviceRequestResults.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                serviceRequestResults,
                paginationDetails,
                message = "Service Request Results Fetched"
            });
        }
    }
}
