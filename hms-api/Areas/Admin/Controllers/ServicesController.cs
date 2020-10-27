using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServices _serviceRepo;
        private readonly IServiceCategory _serviceCategoryRepo;

        public ServicesController(IServices serviceRepo, IServiceCategory serviceCategoryRepo)
        {
            _serviceRepo = serviceRepo;
            _serviceCategoryRepo = serviceCategoryRepo;
        }

        [HttpGet("GetAllServices")]
        public async Task<IActionResult> Services()
        {
            var services = await _serviceRepo.GetAllService();

            if (services.Any())
                return Ok(services);
            else
                return NoContent();
        }

        [HttpPost("CreateAService")]
        public async Task<IActionResult> CreateService(ServiceDtoForCreate serviceDtoForCreate)
        {
            if (serviceDtoForCreate == null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _serviceRepo.AddService(serviceDtoForCreate);
            if (!result)
            {
                return BadRequest(new
                {
                    response = 501,
                    message = "Service failed to create"
                });
            }

            return Ok(new
            {
                message = "Service created successfully"
            });
        }

        [HttpGet("Service/{Id}")]
        public async Task<IActionResult> GetService(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request, Check Id and try again"
                });

            var service = await _serviceRepo.GetById(Id);
            if(service == null)
            {
                return NotFound(new
                {
                    response = 401,
                    message = "Service not found, wrong Id"
                });
            }

            return Ok(new {
            service,
            message="service fetched"});
        }


        [HttpGet("ServiceCategories")]
        public async Task<IActionResult> ServicesCategory()
        {
            var services = await _serviceCategoryRepo.GetCategoriesAsync();

            if (services.Any())
                return Ok(services);
            else
                return NoContent();
        }


        [HttpPost("AddServiceCategory")]
        public async Task<IActionResult> CreateServiceCategory(ServiceCategoryDtoForCreate categoryDtoForCreate)
        {
            if (categoryDtoForCreate == null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _serviceCategoryRepo.AddServiceCategoryAsync(categoryDtoForCreate);
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

        [HttpGet("ServiceCategory/{Id}")]
        public async Task<IActionResult> GetServiceCategory(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request, Check Id and try again"
                });

            var service = await _serviceCategoryRepo.GetServiceCategoryAsync(Id);
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
