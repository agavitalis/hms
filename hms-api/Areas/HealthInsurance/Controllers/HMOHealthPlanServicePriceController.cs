using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Health Plan Service Prices")]
    [ApiController]
    public class HMOHealthPlanServicePriceController : Controller
    {

        private readonly IHMOHealthPlanServicePrice _servicePrice;
        private readonly IMapper _mapper;
        private readonly IServices _service;
        private readonly IHMOHealthPlan _hMOHealthPlan;

        public HMOHealthPlanServicePriceController(IHMOHealthPlanServicePrice servicePrice, IHMOHealthPlan hMOHealthPlan, IServices service, IMapper mapper)
        {
            _servicePrice = servicePrice;
            _mapper = mapper;
            _service = service;
            _hMOHealthPlan = hMOHealthPlan;
        }


        [Route("GetServicePrice")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPrice(string ServicePriceId)
        {
            if (string.IsNullOrEmpty(ServicePriceId))
            {
                return BadRequest();
            }

            var servicePrice = await _servicePrice.GetServicePrice(ServicePriceId);

            if (servicePrice == null)
            {
                return NotFound();
            }

            return Ok(new { servicePrice, mwessage = "Service Price returned" });
        }


        [Route("GetServicePrices")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPrices()
        {
            var drugPrices = await _servicePrice.GetServicePrices();
            return Ok(new { drugPrices, message = "Service Prices Fetched" });
        }

        [Route("GetServicePricesByService")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByDrug(string ServiceId)
        {
            var servicePrices = await _servicePrice.GetServicePricesByService(ServiceId);
            return Ok(new { servicePrices, message = "Service Prices Fetched" });
        }

        [Route("GetServicePricesByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByHealthPlan(string HealthPlanId, [FromQuery] PaginationParameter paginationParameter)
        {
            var servicePrices = _servicePrice.GetServicePricesByHealthPlan(HealthPlanId, paginationParameter);

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
                message = "Service Prices Returned"
            });
        }



        [Route("CreateServicePrice")]
        [HttpPost]
        public async Task<IActionResult> CreateDrugPrice(HMOHealthPlanServicePriceDtoForCreate servicePrice)
        {
            if (servicePrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (string.IsNullOrEmpty(servicePrice.HMOHealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }

            var service = await  _service.GetServiceByIdAsync(servicePrice.ServiceId);
            var hmoHealthPlan = await _hMOHealthPlan.GetHMOHealthPlan(servicePrice.HMOHealthPlanId);

            if (service == null)
            {
                return BadRequest(new { response = "301", message = "Invalid ServiceId" });
            }

            if (hmoHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMO Health Plan Id" });
            }

            var servicePriceToCreate = _mapper.Map<HMOHealthPlanServicePrice>(servicePrice);

            var res = await _servicePrice.CreateServicePrice(servicePriceToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug Price Failed To Create" });
            }

            return Ok(new
            {
                servicePrice,
                message = "Service Price Created Successfully"
            });
        }


        [Route("UpdateServicePrice")]
        [HttpPost]
        public async Task<IActionResult> UpdateDrugPrice(HMOHealthPlanServicePriceDtoForUpdate servicePrice)
        {
            if (servicePrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var service = await _service.GetServiceByIdAsync(servicePrice.ServiceId);
            var hmoHealthPlan = await _hMOHealthPlan.GetHMOHealthPlan(servicePrice.HMOHealthPlanId);

            if (service == null)
            {
                return BadRequest(new { response = "301", message = "Invalid ServiceId" });
            }

            if (hmoHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMO Health Plan Id" });
            }
            var servicePriceToUpdate = _mapper.Map<HMOHealthPlanServicePrice>(servicePrice);

            var res = await _servicePrice.UpdateServicePrice(servicePriceToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug Price Failed To Update" });
            }

            return Ok(new
            {
                servicePrice,
                message = "Service Price Updated Successfully"
            });
        }



        [Route("DeleteServicePrice")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDrug(HMOHealthPlanServicePriceDtoForDelete servicePrice)
        {
            if (servicePrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugPriceToDelete = _mapper.Map<HMOHealthPlanServicePrice>(servicePrice);

            var res = await _servicePrice.DeleteServicePrice(drugPriceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Service Price failed to delete" });
            }

            return Ok(new { servicePrice, message = "Service Price Deleted" });
        }
    }
}
