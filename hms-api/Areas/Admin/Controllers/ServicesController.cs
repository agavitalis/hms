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
    [Route("api/Admin")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServices _serviceRepo;

        public ServicesController(IServices serviceRepo)
        {
            _serviceRepo = serviceRepo;
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
    }
}
