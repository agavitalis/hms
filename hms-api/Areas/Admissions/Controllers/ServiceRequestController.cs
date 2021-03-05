using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
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
    }
}
