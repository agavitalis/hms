using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Service Requests")]
    [ApiController]
    public class AdmissionServiceRequestController : Controller
    {
        private readonly IServices _serviceRepo;
        private readonly IMapper _mapper;
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmissionServiceRequest _admissionServiceRequest;
        private readonly IDrugInvoicing _drugInvoicing;
        private readonly IDrug _drug;


        public AdmissionServiceRequestController(IServices serviceRepo, IMapper mapper, IAdmission admission, IAdmissionInvoice admissionInvoice, IAdmissionServiceRequest admissionServiceRequest, IDrug drug, IDrugInvoicing drugInvoicing)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
            _admission = admission;
            _drug = drug;
            _admissionInvoice = admissionInvoice;
            _admissionServiceRequest = admissionServiceRequest;
            _drugInvoicing = drugInvoicing;
        }

        [HttpPost("RequestService")]
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



        //[HttpGet("GetServicesInAnInvoice/{invoiceId}")]
        //public async Task<IActionResult> GetServiceRequestInAnInvoice(string invoiceId, [FromQuery] PaginationParameter paginationParameter)
        //{
        //    var serviceRequests = _admissionServiceRequest.GetServiceRequestsInAnInvoicePagination(invoiceId, paginationParameter);

        //    if (!serviceRequests.Any())
        //        return BadRequest(new
        //        {
        //            response = "400",
        //            message = "No Service Requests Found with this ID"
        //        });

        //    var paginationDetails = new
        //    {
        //        serviceRequests.TotalCount,
        //        serviceRequests.PageSize,
        //        serviceRequests.CurrentPage,
        //        serviceRequests.TotalPages,
        //        serviceRequests.HasNext,
        //        serviceRequests.HasPrevious
        //    };

        //    //This is optional
        //    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

        //    return Ok(new
        //    {
        //        serviceRequests,
        //        paginationDetails,
        //        message = "Service Requests Fetched"
        //    });
        //}

        //[HttpGet("GetPatientServiceRequestInvoices/{patientId}")]
        //public async Task<IActionResult> GetPatientInvoiceWithPagination(string patientId, [FromQuery] PaginationParameter paginationParameter)
        //{

        //    var patientInvoices = _serviceRepo.GetServiceInvoiceForPatient(patientId, paginationParameter);
        //    if (patientInvoices == null)
        //    {
        //        return BadRequest(new
        //        {
        //            message = "Invalid Patient Id"
        //        });

        //    }
        //    var paginationDetails = new
        //    {
        //        patientInvoices.TotalCount,
        //        patientInvoices.PageSize,
        //        patientInvoices.CurrentPage,
        //        patientInvoices.TotalPages,
        //        patientInvoices.HasNext,
        //        patientInvoices.HasPrevious
        //    };

        //    //This is optional
        //    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));




        //    if (!patientInvoices.Any())
        //        return Ok(new
        //        {
        //            response = "204",
        //            message = "No Service Request in this patient"
        //        });

        //    return Ok(new
        //    {
        //        patientInvoices,
        //        paginationDetails,
        //        message = "List of request in invoice fetched"
        //    });
        //}

        //[HttpGet("GetServiceRequest/{ServiceRequestId}")]
        //public async Task<IActionResult> GetServiceRequest(string ServiceRequestId)
        //{
        //    var serviceRequest = await _serviceRepo.GetServiceRequest(ServiceRequestId);
        //    if (serviceRequest == null)
        //    {
        //        return BadRequest(new { message = "Invalid post attempt" });
        //    }

        //    return Ok(new
        //    {
        //        serviceRequest,
        //        message = "Service Request"
        //    });
        //}
    }
}
