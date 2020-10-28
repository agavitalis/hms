using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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

        [HttpPost("CreateAService")]
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

        [HttpGet("GetAService/{Id}")]
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


        [HttpGet("GetAllServiceCategories")]
        public async Task<IActionResult> ServicesCategory()
        {
            var services = await _serviceRepo.GetAllServiceCategories();

            //if (services.Any())
                return Ok(services);
           // else
           //  return NoContent();
        }


        [HttpPost("CreateAServiceCategory")]
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

        [HttpGet("GetAServiceCategory/{Id}")]
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
    }
}
