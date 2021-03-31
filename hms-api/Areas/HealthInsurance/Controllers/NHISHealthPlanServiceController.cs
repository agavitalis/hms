using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage NHIS Health Plan Services")]
    [ApiController]
    public class NHISHealthPlanServiceController : Controller
    {
        private readonly INHISHealthPlanService _healthPlanService;
        private readonly IMapper _mapper;
        private readonly IServices _service;
        private readonly INHISHealthPlan _NHISHealthPlan;

        public NHISHealthPlanServiceController(INHISHealthPlanService healthPlanService, INHISHealthPlan NHISHealthPlan, IServices service, IMapper mapper)
        {
            _NHISHealthPlan = NHISHealthPlan;
            _service = service;
            _healthPlanService = healthPlanService;
            _mapper = mapper;
        }


        [Route("GetHealthPlanService")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanService(string HealthPlanServiceId)
        {
            if (string.IsNullOrEmpty(HealthPlanServiceId))
            {
                return BadRequest();
            }

            var healthPlanService = await _healthPlanService.GetHealthPlanService(HealthPlanServiceId);

            if (healthPlanService == null)
            {
                return NotFound();
            }

            return Ok(new { healthPlanService, mwessage = "Health Plan Service Returned" });
        }


        [Route("GetHealthPlanServices")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanServices()
        {
            var healthPlanServices = await _healthPlanService.GetHealthPlanServices();
            return Ok(new { healthPlanServices, message = "Health Plan Services Returned" });
        }

        [Route("GetHealthPlanServicesByService")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanServicesByService(string ServiceId)
        {
            var healthPlanServices = await _healthPlanService.GetHealthPlanServicesByService(ServiceId);
            return Ok(new { healthPlanServices, message = "Health Plan Services Returned" });
        }


        [Route("GetHealthPlanServicesByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByHealthPlan(string HealthPlanId, [FromQuery] PaginationParameter paginationParameter)
        {
            var servicePrices = _healthPlanService.GetHealthPlanServicesByHealthPlan(HealthPlanId, paginationParameter);

            var paginationDetails = new
            {
                servicePrices.TotalCount,
                servicePrices.PageSize,
                servicePrices.CurrentPage,
                servicePrices.TotalPages,
                servicePrices.HasNext,
                servicePrices.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                servicePrices,
                paginationDetails,
                message = "Services Returned"
            });
        }



        [Route("CreateHealthPlanService")]
        [HttpPost]
        public async Task<IActionResult> CreateHealthPlanService(NHISHealthPlanServiceDtoForCreate HealthPlanService)
        {
            if (HealthPlanService == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (string.IsNullOrEmpty(HealthPlanService.NHISHealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HealthPlanServiceToCreate = _mapper.Map<NHISHealthPlanService>(HealthPlanService);

            var res = await _healthPlanService.CreateHealthPlanService(HealthPlanServiceToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Service Failed To Create" });
            }

            return Ok(new
            {
                HealthPlanService,
                message = "Health Plan Service Created Successfully"
            });
        }


        [Route("UpdateHealthPlanService")]
        [HttpPost]
        public async Task<IActionResult> UpdateHealthPlanService(NHISHealthPlanServiceDtoForUpdate HealthPlanService)
        {
            if (HealthPlanService == null || HealthPlanService.Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var service = await  _service.GetServiceByIdAsync(HealthPlanService.ServiceId);
            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(HealthPlanService.NHISHealthPlanId);

            if (service == null)
            {
                return BadRequest(new { response = "301", message = "Invalid ServiceId" });
            }

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid NHIS Health Plan Id" });
            }
            var HealthPlanServiceToUpdate = _mapper.Map<NHISHealthPlanService>(HealthPlanService);

            var res = await _healthPlanService.UpdateHealthPlanService(HealthPlanServiceToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Service Failed To Update" });
            }

            return Ok(new
            {
                HealthPlanService,
                message = "HealthPlan Drug Updated Successfully"
            });
        }



        [Route("DeleteHealthPlanService")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHealthPlanService(NHISHealthPlanServiceDtoForDelete HealthPlanService)
        {
            if (HealthPlanService == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanServiceToDelete = _mapper.Map<NHISHealthPlanService>(HealthPlanService);

            var res = await _healthPlanService.DeleteHealthPlanService(healthPlanServiceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Service failed to delete" });
            }

            return Ok(new { HealthPlanService, message = "Health Plan Service Deleted" });
        }
    }
}
