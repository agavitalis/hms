using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy - Manage Drug Prices")]
    [ApiController]
    public class DrugPriceController : Controller
    {
        private readonly IDrugPrice _drugPrice;
        private readonly IMapper _mapper;
        private readonly IDrug _drug;

        public DrugPriceController(IDrugPrice drugPrice, IDrug drug, IMapper mapper)
        {
            _drugPrice = drugPrice;
            _mapper = mapper;
            _drug = drug;
        }


        [Route("GetDrugPrice/{DrugPriceId}")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPrice(string DrugPriceId)
        {
            if (DrugPriceId == "")
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
        public async Task<IActionResult> GetDrugPricesByHealthPlan(string HealthPlanId)
        {
            var drugPrices = await _drugPrice.GetDrugPricesByHealthPlan(HealthPlanId);
            return Ok(new { drugPrices, message = "Drugs Prices Fetched" });
        }



        [Route("CreateDrugPrice")]
        [HttpPost]
        public async Task<IActionResult> CreateDrugPrice(DrugPriceDtoForCreate drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (string.IsNullOrEmpty(drugPrice.HealthPlanId))
            {
                var drug = await _drug.GetDrug(drugPrice.DrugId);
                if (drug == null)
                {
                    return BadRequest(new { message = "Invalid Drug Id" });
                }
                drug.DefaultPricePerUnit = drugPrice.PricePerUnit;
                drug.DefaultPricePerContainer = drugPrice.PricePerContainer;
                drug.DefaultPricePerCarton = drugPrice.PricePerCarton;
                var result = await _drug.UpdateDrug(drug);
                if (!result)
                {
                    return BadRequest(new { response = "301", message = "Failed to Update Drug Default Price" });
                }

                return Ok(new
                {
                    drug,
                    message = "Drug Default Price Updated Successfully"
                });
            }
            var drugPriceToCreate = _mapper.Map<DrugPrice>(drugPrice);

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

        [Route("UpdateDefaultDrugPrice")]
        [HttpPost]
        public async Task<IActionResult> UpdateDefaultDrugPrice(DefaultDrugPriceDtoForUpdate drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drug = await _drug.GetDrug(drugPrice.DrugId);
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid Drug Id"});
            }
            drug.DefaultPricePerUnit = drugPrice.DefaultPricePerUnit;
            drug.DefaultPricePerContainer = drugPrice.DefaultPricePerContainer;
            drug.DefaultPricePerCarton = drugPrice.DefaultPricePerCarton;
            var res = await _drug.UpdateDrug(drug);

            
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to update" });
            }

            return Ok(new
            {
                drugPrice,
                message = "Drug Price Updated successfully"
            });
        }

        [Route("UpdateDrugPrice")]
        [HttpPost]
        public async Task<IActionResult> UpdateDrugPrice(DrugPriceDtoForUpdate drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var DrugPrice = await _drug.GetDrug(drugPrice.DrugId);
            
            if (DrugPrice == null)
            {
                return BadRequest(new { message = "Invalid Drug Price Id" });
            }
           
            var drugPriceToUpdate = _mapper.Map<DrugPrice>(drugPrice);

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
        public async Task<IActionResult> DeleteDrug(DrugPriceDtoForDelete drugPrice)
        {
            if (drugPrice == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugPriceToDelete = _mapper.Map<DrugPrice>(drugPrice);

            var res = await _drugPrice.DeleteDrugPrice(drugPriceToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to delete" });
            }

            return Ok(new { drugPrice, message = "Drug Deleted" });
        }
    }
}
