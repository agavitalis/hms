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
    [Route("api/Admin", Name = "Admin - Manage Service")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServices _serviceRepo;
        private readonly IServiceRequest _serviceRequest;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patientRepo;

        public ServicesController(IServices serviceRepo, IServiceRequest serviceRequest, IMapper mapper, IPatientProfile patientRepo)
        {
            _serviceRepo = serviceRepo;
            _serviceRequest = serviceRequest;
            _mapper = mapper;
            _patientRepo = patientRepo;
        }

        [HttpGet("GetAllServices")]
        public async Task<IActionResult> Services()
        {
            var services = await _serviceRepo.GetAllServices();

            return Ok(services);
           
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

            var service = await _serviceRepo.GetServiceByIdAsync(serviceDtoForDelete.Id);
            if (service == null)
            {
                return BadRequest(new { message = "Invalid Service Id" });
            }
            var services = await _serviceRequest.GetServiceRequestByServiceAsync(serviceDtoForDelete.Id);
            if (services.Any())
            {
                return BadRequest(new { message = "Service Has Requests Tied To It and Cannot Be Deleted" });
            }

            var serviceToDelete = _mapper.Map<Service>(serviceDtoForDelete);

            var res = await _serviceRepo.DeleteService(serviceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to delete" });
            }

            return Ok(new { serviceToDelete, message = "Service Deleted" });
        }

       
    }
}
