using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServices _serviceRepo;
        private readonly IMapper _mapper;

        public ServicesController(IServices serviceRepo, IMapper mapper)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;

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

        [HttpPost("DeleteService", Name = "deleteService")]
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

       
        [HttpPost("UpdateServiceCategory", Name = "updateServiceCategory")]
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

        [HttpPost("DeleteServiceCategory", Name = "deleteServiceCategory")]
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

    }
}
