using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy - Manage Drugs")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly IDrug _drug;
        private readonly IMapper _mapper;

        public DrugController(IDrug drug, IMapper mapper)
        {
            _drug = drug;
            _mapper = mapper;
        }

        [Route("GetDrugsCount")]
        [HttpGet]
        public async Task<IActionResult> GetDrugsCount()
        {
            var drugCount = await _drug.GetDrugCount();

            return Ok(new
            {
                drugCount,
                message = "Drugs Count"
            });
        }

        [Route("GetDrug/{DrugId}")]
        [HttpGet]
        public async Task<IActionResult> GetDrug(string DrugId)
        {
            if (DrugId == "")
            {
                return BadRequest();
            }

            var drug = await _drug.GetDrug(DrugId);

            if (drug == null)
            {
                return NotFound();
            }

            return Ok(new { drug, mwessage = "Drug returned" });
        }

        //[Route("GetAllDrugs")]
        //[HttpGet]
        //public async Task<IActionResult> GetDrugs()
        //{
        //    var drugs = await _drug.GetDrugs();

        //    return Ok(new { drugs, message = "Drugs Fetched" });

        //}

        [Route("GetAllDrugs")]
        [HttpGet]
        public async Task<IActionResult> GetAllDrugs([FromQuery] PaginationParameter paginationParameter)
        {
            var drugs = _drug.GetDrugsPagination(paginationParameter);

            var paginationDetails = new
            {
                drugs.TotalCount,
                drugs.PageSize,
                drugs.CurrentPage,
                drugs.TotalPages,
                drugs.HasNext,
                drugs.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                drugs,
                paginationDetails,
                message = "Drugs Fetched"
            });
        }

        [Route("GetDrugsByDrugType")]
        [HttpGet]
        public async Task<IActionResult> GetDrugsByDrugType(string DrugType,[FromQuery] PaginationParameter paginationParameter)
        {
            var drugs = _drug.GetDrugsByDrugType(DrugType, paginationParameter);

            var paginationDetails = new
            {
                drugs.TotalCount,
                drugs.PageSize,
                drugs.CurrentPage,
                drugs.TotalPages,
                drugs.HasNext,
                drugs.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                drugs,
                paginationDetails,
                message = "Drugs Fetched"
            });
        }


        [Route("SearchDrugs")]
        [HttpGet]
        public async Task<IActionResult> SearchDrugs(string searchString)
        {
            var drugs = await _drug.SearchDrugs(searchString);
            return Ok(new { drugs, message = "Drugs Fetched" });

        }

        [Route("RegisterDrug")]
        [HttpPost]
        public async Task<IActionResult> CreateDrug(DrugDtoForCreate drug)
        {
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugToCreate = _mapper.Map<Drug>(drug);

            var res = await _drug.CreateDrug(drugToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to create" });
            }

            return Ok(new
            {
                drug,
                message = "Drug created successfully"
            });
        }

        [Route("RegisterExistingDrug")]
        [HttpPost]
        public async Task<IActionResult> CreateExistingDrug(DrugDtoForCreateExistingDrug Drug)
        {
            if (Drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drug = await _drug.GetDrug(Drug.DrugId);

            if (drug == null)
            {
                return NotFound();
            }

            var newDrug = new Drug()
            {
                SKU = Drug.SKU,
                Name = drug.Name,
                GenericName = drug.GenericName,
                Manufacturer = drug.Manufacturer,
                Measurment = drug.Measurment,
                ExpiryDate = Drug.ExpiryDate,
                DrugType = drug.DrugType,
                QuantityInStock = Drug.QuantityInStock,
                CostPricePerContainer = drug.CostPricePerContainer,
                QuantityPerContainer = drug.QuantityPerContainer,
                ContainersPerCarton = drug.ContainersPerCarton,
                DefaultPricePerUnit = drug.DefaultPricePerUnit,
                DefaultPricePerContainer = drug.DefaultPricePerContainer,
                DefaultPricePerCarton = drug.DefaultPricePerCarton
            };

            

            var res = await _drug.CreateDrug(newDrug);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to create" });
            }

            return Ok(new
            {
                newDrug,
                message = "Drug created successfully"
            });
        }

        [Route("UpdateDrug")]
        [HttpPost]
        public async Task<IActionResult> UpdateDrug([FromBody]DrugDtoForUpdate Drug)
        {
             var drug = await _drug.GetDrug(Drug.Id);
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            drug.SKU = Drug.SKU;
            drug.Name = Drug.Name;
            drug.GenericName = Drug.GenericName;
            drug.Manufacturer = Drug.Manufacturer;
            drug.Measurment = Drug.Measurment;
            drug.CostPricePerContainer = Drug.CostPricePerContainer;
            drug.QuantityPerContainer = Drug.QuantityPerContainer;
            drug.ContainersPerCarton = Drug.ContainersPerCarton;
            drug.ExpiryDate = Drug.ExpiryDate;

            

            var res = await _drug.UpdateDrug(drug);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to update" });
            }

            return Ok(new
            {
                drug,
                message = "Drug updated successfully"
            });
        }

        [Route("UpdateDrugQuantity")]
        [HttpPost]
        public async Task<IActionResult> UpdateDrugQuantity(string DrugId, int DrugQuantity)
        {

            var drug = await _drug.GetDrug(DrugId);
            
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            drug.QuantityInStock += DrugQuantity;
           //then we patch
            await _drug.UpdateDrug(drug);

            return Ok(new
            {
                drug,
                message = "Drug Quantity updated successfully"
            });
        }

        [Route("DeleteDrug")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDrug(DrugDtoForDelete drug)
        {
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugToDelete = _mapper.Map<Drug>(drug);

            var res = await _drug.DeleteDrug(drugToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug failed to delete" });
            }

            return Ok(new { drug, message = "Drug Deleted" });
        }
    }
}
