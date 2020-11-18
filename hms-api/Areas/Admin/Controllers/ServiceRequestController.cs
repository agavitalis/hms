using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Service Requests")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServices _serviceRepo;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patientRepo;

        public ServiceRequestController(IServices serviceRepo, IMapper mapper, IPatientProfile patientRepo)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
            _patientRepo = patientRepo;
        }


        [HttpPost("RequestServices")]
        public async Task<IActionResult> RequestServices(ServiceRequestDtoForCreate serviceRequest)
        {
            if (serviceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            //check if the patient exist
            var patient = await _patientRepo.GetPatientByIdAsync(serviceRequest.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid patient Id passed, Patient not found",
                });

            serviceRequest.PatientId = patient.PatientId;
            //check if all service id passed exist
            var servicesCheck = await _serviceRepo.CheckIfServicesExist(serviceRequest.ServiceId);
            if (!servicesCheck)
                return BadRequest(new
                {
                    response = "301",
                    message = "One or more service id passed is/are invalid"
                });

            //generate invoice for request
            var invoiceId = await _serviceRepo.GenerateInvoiceForServiceRequest(serviceRequest);
            if (string.IsNullOrEmpty(invoiceId))
                return BadRequest(new
                {
                    response = "301",
                    message = "Failed to generate invoice !!!, Try Again"
                });

            //insert request
            var result = await _serviceRepo.CreateServiceRequest(serviceRequest, invoiceId);
            if(!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Request Service Failed !!!, Try Again"
                });

            return Ok(new { message="Service Request submitted successfully"});
        }


        
        [HttpPost("GetAllServiceRequestInvoice")]
        public async Task<IActionResult> GetAllServiceInvoiceWithPagination(PaginationParameter paginationParameter)
        {
            var serviceInvoices = await _serviceRepo.GetServiceInvoices(paginationParameter);

            return Ok(new
            {
                serviceInvoices,
                message = "List of invoice fetched"
            });
        }

        [HttpGet("GetAllServiceRequestInvoice")]
        public async Task<IActionResult> GetAllServiceInvoice()
        {
            var serviceInvoices = await _serviceRepo.GetServiceInvoices();

            return Ok(new
            {
                serviceInvoices,
                message = "List of invoice fetched"
            });
        }


        [HttpGet("GetServicesInAnInvoice/{invoiceId}")]
        public async Task<IActionResult> GetServiceRequestInAnInvoice(string invoiceId)
        {
            var serviceRequest = await _serviceRepo.GetServiceRequestInAnInvoice(invoiceId);
            if (!serviceRequest.Any())
                return BadRequest(new
                {
                    response = "400",
                    message = "No Service Found with this ID"
                });

            return Ok(new
            {
                serviceRequest,
                message = "List of request in invoice fetched"
            });
        }

        [HttpPost("GetPatientServiceRequestInvoices/{patientId}")]
        public async Task<IActionResult> GetPatientInvoiceWithPagination(string patientId, PaginationParameter paginationParameter)
        {
            var patientInvoices = await _serviceRepo.GetServiceInvoiceForPatient(patientId, paginationParameter);
            if (!patientInvoices.Any())
                return Ok(new
                {
                    response = "204",
                    message = "No Service Request in this patient"
                });

            return Ok(new
            {
                patientInvoices,
                message = "List of request in invoice fetched"
            });
        }

        [HttpGet("GetPatientServiceRequestInvoices/{patientId}")]
        public async Task<IActionResult> GetPatientInvoice(string patientId)
        {
            var patientInvoices = await _serviceRepo.GetServiceInvoiceForPatient(patientId);
            if (!patientInvoices.Any())
                return Ok(new
                {
                    response = "204",
                    message = "No Service Request in this patient"
                });

            return Ok(new
            {
                patientInvoices,
                message = "List of request in invoice fetched"
            });
        }

        [HttpGet("GetServiceRequest/{ServiceRequestId}")]
        public async Task<IActionResult> GetServiceRequest(string ServiceRequestId)
        {
            var serviceRequest = await _serviceRepo.GetServiceRequest(ServiceRequestId);
            if (serviceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            return Ok(new
            {
                serviceRequest,
                message = "Service Request"
            });
        }

        [HttpGet("DeleteServiceRequestFromInvoice/{ServiceRequestId}")]
        public async Task<IActionResult> DeleteServiceRequestFromInvoice(string ServiceRequestId)
        {
            var serviceRequestResult = await _serviceRepo.GetServiceRequestResults(ServiceRequestId);
            
            if (serviceRequestResult.Any())
            {
                return BadRequest(new { message = "This Request Already Has a Result Associated With It And Cannot Be Deleted" });
            }
            
            var serviceRequest = await _serviceRepo.GetServiceRequest(ServiceRequestId);
            
            if (serviceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (serviceRequest.ServiceInvoice.PaymentStatus == "PAID")
            {
                return BadRequest(new { message = "Payment For This Invoice Is Already Complete" });
            }

            var serviceRequestPrice = serviceRequest.Amount;
            serviceRequest.ServiceInvoice.AmountTotal = serviceRequest.ServiceInvoice.AmountTotal - serviceRequestPrice;
            bool res1 = await _serviceRepo.UpdateServiceRequestInvoice(serviceRequest.ServiceInvoice);
            bool res = await _serviceRepo.DeleteServiceRequest(serviceRequest);
            return Ok(new
            {
                serviceRequest.ServiceInvoice,
                message = "Service Request Deleted Succesfully"
            });
        }

        [HttpPost("PayForServices")]
        public async Task<IActionResult> PayForServices(ServiceRequestPaymentDto services)
        {
            if (services == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var patient = await _patientRepo.GetPatientByIdAsync(services.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid patient Id passed, Patient not found",
                });

            //check if all serviceRequest id passed exist
            var servicesRequestCheck = await _serviceRepo.CheckIfServiceRequestIdExist(services.ServiceRequestId);
            if (!servicesRequestCheck)
                return BadRequest(new
                {
                    response = "301",
                    message = "One or more Service Request Id Passed is/are invalid"
                });

            //check if the amount is correct
            var correctAmount = await _serviceRepo.CheckIfAmountPaidIsCorrect(services);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the services paid for does not match"
                });

            //pay for services
            var result = await _serviceRepo.PayForServices(services);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Payment for these servies cannot be completed, pls contact the Admins"
                });

            return Ok(new { message = "Payment for services completed successfully" });
        }

        [HttpPost("UploadServiceRequestResult")]
        public async Task<IActionResult> UploadServiceRequestResult([FromForm]ServiceUploadResultDto serviceRequestResultForUpload)
        {
            if (serviceRequestResultForUpload == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceRequest = await _serviceRepo.GetServiceRequest(serviceRequestResultForUpload.ServiceRequestId);

            if (serviceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            
            var serviceRequestResultToUpload = _mapper.Map<ServiceRequestResult>(serviceRequestResultForUpload);

            var serviceRequestResult = await _serviceRepo.UploadServiceRequestResult(serviceRequestResultToUpload);
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
            var serviceRequestResultImage = await _serviceRepo.UploadServiceRequestResultImage(serviceRequestResultForUpload, serviceRequestResult.Id);
            return Ok(new
            {
                serviceRequestResult,
                serviceRequestResultImage,
                message = "Result Uploaded Successfully"
            });
        }

        [HttpGet("GetServiceRequestResults/{ServiceRequestId}")]
        public async Task<IActionResult> GetServiceRequestResults(string ServiceRequestId)
        {
            
            if (ServiceRequestId == null)
            {
                return BadRequest(new { message = "Service Request Id Not Passed" });
            }

            var serviceRequestResults = await _serviceRepo.GetServiceRequestResults(ServiceRequestId);
            return Ok(new
            {
                serviceRequestResults,
                message = "Service Request Results"
            });
        }

        [HttpGet("GetServiceRequestResultsForPatient/{patientId}")]
        public async Task<IActionResult> GetServiceRequestResultsForPatient(string patientId)
        {
            var results = await _serviceRepo.GetServiceRequestResultsForPatient(patientId);
            
            return Ok(new
            {
                results,
                message = "results of services requested"
            });
        }
    }
}
