﻿using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Health Plan Drug Prices")]
    [ApiController]
    public class HMOHealthplanDrugPriceController : Controller
    {
        private readonly IHMOHealthPlanDrugPrice _drugPrice;
        private readonly IMapper _mapper;
        private readonly IDrug _drug;
        private readonly IHMOHealthPlan _hMOHealthPlan;

        public HMOHealthplanDrugPriceController(IHMOHealthPlanDrugPrice drugPrice, IHMOHealthPlan hMOHealthPlan, IDrug drug, IMapper mapper)
        {
            _hMOHealthPlan = hMOHealthPlan;
            _drugPrice = drugPrice;
            _mapper = mapper;
            _drug = drug;
        }


        [Route("GetDrugPrice/{DrugPriceId}")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPrice(string DrugPriceId)
        {
            if (string.IsNullOrEmpty(DrugPriceId))
            {
                return BadRequest();
            }

            var drugPrice = await _drugPrice.GetDrugPrice(DrugPriceId);

            if (drugPrice == null)
            {
                return NotFound();
            }

            return Ok(new { drugPrice, mwessage = "Drug Price returned" });
        }


        [Route("GetDrugPrices")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPrices()
        {
            var drugPrices = await _drugPrice.GetDrugPrices();
            return Ok(new { drugPrices, message = "Drugs Prices Fetched" });
        }

        [Route("GetDrugPricesByDrug")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByDrug(string DrugId)
        {
            var drugPrices = await _drugPrice.GetDrugPricesByDrug(DrugId);
            return Ok(new { drugPrices, message = "Drugs Prices Fetched" });
        }

        

        [Route("GetDrugPricesByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByHealthPlan(string HealthPlanId, [FromQuery] PaginationParameter paginationParameter)
        {
            var drugPrices = _drugPrice.GetDrugPricesByHealthPlan(HealthPlanId, paginationParameter);

            var paginationDetails = new
            {
                drugPrices.TotalCount,
                drugPrices.PageSize,
                drugPrices.CurrentPage,
                drugPrices.TotalPages,
                drugPrices.HasNext,
                drugPrices.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                drugPrices,
                paginationDetails,
                message = "Service Prices Returned"
            });
        }


        [Route("CreateDrugPrice")]
        [HttpPost]
        public async Task<IActionResult> CreateDrugPrice(HMOHealthPlanDrugPriceDtoForCreate drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (string.IsNullOrEmpty(drugPrice.HMOHealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }

            var drug = await _drug.GetDrug(drugPrice.DrugId);
            var hmoHealthPlan = await _hMOHealthPlan.GetHMOHealthPlan(drugPrice.HMOHealthPlanId);

            if (drug == null)
            {
                return BadRequest(new { response = "301", message = "Invalid DrugId" });
            }

            if (hmoHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMO Health Plan Id" });
            }
            var drugPriceToCreate = _mapper.Map<HMOHealthPlanDrugPrice>(drugPrice);

            var res = await _drugPrice.CreateDrugPrice(drugPriceToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug Price Failed To Create" });
            }

            return Ok(new
            {
                drugPrice,
                message = "Drug Price Created Successfully"
            });
        }


        [Route("UpdateDrugPrice")]
        [HttpPost]
        public async Task<IActionResult> UpdateDrugPrice(HMOHealthPlanDrugPriceDtoForUpdate drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var drug = await  _drug.GetDrug(drugPrice.DrugId);
            var hmoHealthPlan = await _hMOHealthPlan.GetHMOHealthPlan(drugPrice.HMOHealthPlanId);

            if (drug == null)
            {
                return BadRequest(new { response = "301", message = "Invalid DrugId" });
            }

            if (hmoHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMO Health Plan Id" });
            }
            var drugPriceToUpdate = _mapper.Map<HMOHealthPlanDrugPrice>(drugPrice);

            var res = await _drugPrice.UpdateDrugPrice(drugPriceToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug Price Failed To Update" });
            }

            return Ok(new
            {
                drugPrice,
                message = "Drug Price Updated Successfully"
            });
        }



        [Route("DeleteDrugPrice")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDrug(HMOHealthPlanDrugPriceDtoForDelete drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugPriceToDelete = _mapper.Map<HMOHealthPlanDrugPrice>(drugPrice);

            var res = await _drugPrice.DeleteDrugPrice(drugPriceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to delete" });
            }

            return Ok(new { drugPrice, message = "Drug Price Deleted" });
        }
    }
}
