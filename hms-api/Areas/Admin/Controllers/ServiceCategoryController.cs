using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Services Categories")]
    [ApiController]
    public class ServiceCategoryController : Controller
    {
        private readonly IServiceCategory _serviceCategoryRepo;
        private readonly IMapper _mapper;
        private readonly IServices _service;

        public ServiceCategoryController(IServiceCategory serviceCategoryRepo, IMapper mapper, IServices service)
        {
            _serviceCategoryRepo = serviceCategoryRepo;
            _mapper = mapper;
            _service = service;
        }


        [HttpGet("GetAllServiceCategories")]
        public async Task<IActionResult> ServicesCategory()
        {
            var services = await _serviceCategoryRepo.GetAllServiceCategories();

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

            var service = await _serviceCategoryRepo.GetServiceCategoryByIdAsync(Id);
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
            var result = await _serviceCategoryRepo.CreateServiceCategoryAsync(serviceCategoryToCreate);
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

            var res = await _serviceCategoryRepo.UpdateServiceCategory(serviceCategoryToUpdate);
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

            var serviceCategory = await _serviceCategoryRepo.GetServiceCategoryByIdAsync(serviceCategoryDtoForDelete.Id);
            if (serviceCategory == null)
            {
                return BadRequest(new { message = "Invalid Service Category Id" });
            }
            var services = await _service.GetServiceByCategoryAsync(serviceCategoryDtoForDelete.Id);
            if (services.Any())
            {
                return BadRequest(new { message = "Service Category Has Services Tied To It and Cannot Be Deleted" });
            }
            var serviceCategoryToDelete = _mapper.Map<ServiceCategory>(serviceCategoryDtoForDelete);

            var res = await _serviceCategoryRepo.DeleteServiceCategory(serviceCategoryToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Service Category failed to delete" });
            }

            return Ok(new { serviceCategoryToDelete, message = "Service Category Deleted" });
        }

        [HttpGet("GetAllServicesInAServiceCategory")]
        public async Task<IActionResult> GetAllServicesInAServiceCategory(string serviceCategoryId)
        {
            var services = await _serviceCategoryRepo.GetAllServicesInAServiceCategory(serviceCategoryId);
            return Ok(services);

        }

    }
}
