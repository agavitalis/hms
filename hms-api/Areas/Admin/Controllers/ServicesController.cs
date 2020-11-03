using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin- Manage Services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServices _serviceRepo;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patientRepo;

        public ServicesController(IServices serviceRepo, IMapper mapper, IPatientProfile patientRepo)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
            _patientRepo = patientRepo;
        }

        [HttpGet("GetAllServices")]
        public async Task<IActionResult> Services()
        {
            var services = await _serviceRepo.GetAllServices();

            //if (services.Any())
            return Ok(services);
            //else
            //    return NoContent();
        }

        [HttpGet("GetService/{Id}")]
        public async Task<IActionResult> GetService(string Id)
        {
            if (Id == "")
            {
                return BadRequest();
            }

            var res = await _serviceRepo.GetServiceByIdAsync(Id);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Service returned" });


        }


        [HttpPost("CreateService")]
        public async Task<IActionResult> CreateService(ServiceDtoForCreate serviceDtoForCreate)
        {
            if (serviceDtoForCreate == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceToCreate = _mapper.Map<Service>(serviceDtoForCreate);

            var res = await _serviceRepo.CreateService(serviceToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Service failed to create" });
            }

            return Ok(new
            {
                serviceToCreate,
                message = "Service created successfully"
            });
        }

        [HttpPost("UpdateService")]
        public async Task<IActionResult> UpdateService(ServiceDtoForUpdate serviceDtoForUpdate)
        {
            if (serviceDtoForUpdate == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceToUpdate = _mapper.Map<Service>(serviceDtoForUpdate);

            var res = await _serviceRepo.UpdateService(serviceToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Service failed to update" });
            }

            return Ok(new
            {
                serviceToUpdate,
                message = "Service updated successfully"
            });
        }

        [HttpPost("DeleteService")]
        public async Task<IActionResult> DeleteService(ServiceDtoForDelete serviceDtoForDelete)
        {
            if (serviceDtoForDelete == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceToDelete = _mapper.Map<Service>(serviceDtoForDelete);

            var res = await _serviceRepo.DeleteService(serviceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to delete" });
            }

            return Ok(new { serviceToDelete, message = "Service Deleted" });
        }


        [HttpGet("GetAllServiceCategories")]
        public async Task<IActionResult> ServicesCategory()
        {
            var services = await _serviceRepo.GetAllServiceCategories();

            //if (services.Any())
            return Ok(services);
            // else
            //  return NoContent();
        }

        [HttpGet("GetServiceCategory/{Id}")]
        public async Task<IActionResult> GetServiceCategory(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request, Check Id and try again"
                });

            var service = await _serviceRepo.GetServiceCategoryByIdAsync(Id);
            if (service == null)
            {
                return NotFound(new
                {
                    response = 401,
                    message = "Service Category not found, wrong Id"
                });
            }

            return Ok(new
            {
                service,
                message = "service Category fetched"
            });
        }


        [HttpPost("CreateServiceCategory")]
        public async Task<IActionResult> CreateServiceCategory(ServiceCategoryDtoForCreate categoryDtoForCreate)
        {
            if (categoryDtoForCreate == null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var serviceCategoryToCreate = _mapper.Map<ServiceCategory>(categoryDtoForCreate);
            var result = await _serviceRepo.CreateServiceCategoryAsync(serviceCategoryToCreate);
            if (!result)
            {
                return BadRequest(new
                {
                    response = 501,
                    message = "Service Category failed to create"
                });
            }

            return Ok(new
            {
                message = "Service Category created successfully"
            });
        }

       
        [HttpPost("UpdateServiceCategory")]
        public async Task<IActionResult> UpdateServiceCategory(ServiceCategoryDtoForUpdate serviceCategoryDtoForUpdate)
        {
            if (serviceCategoryDtoForUpdate == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceCategoryToUpdate = _mapper.Map<ServiceCategory>(serviceCategoryDtoForUpdate);

            var res = await _serviceRepo.UpdateServiceCategory(serviceCategoryToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to update" });
            }

            return Ok(new
            {
                serviceCategoryToUpdate,
                message = "Service updated successfully"
            });
        }

        [HttpPost("DeleteServiceCategory")]
        public async Task<IActionResult> DeleteServiceCategory(ServiceCategoryDtoForDelete serviceCategoryDtoForDelete)
        {
            if (serviceCategoryDtoForDelete == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceCategoryToDelete = _mapper.Map<ServiceCategory>(serviceCategoryDtoForDelete);

            var res = await _serviceRepo.DeleteServiceCategory(serviceCategoryToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Service Category failed to delete" });
            }

            return Ok(new { serviceCategoryToDelete, message = "Service Category Deleted" });
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

            //check if all service id passed exist
            var servicesCheck = await _serviceRepo.CheckIfServicesExist(serviceRequest.ServiceId);
            if (servicesCheck)
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

    }
}
