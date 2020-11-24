using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetDrug/{Id}")]
        public async Task<IActionResult> GetDrug(string Id)
        {
            if (Id == "")
            {
                return BadRequest();
            }

            var drug = await _drug.GetDrug(Id);

            if (drug == null)
            {
                return NotFound();
            }

            return Ok(new { drug, mwessage = "Drug returned" });
        }

        [HttpGet("GetDrugs")]
        public async Task<IActionResult> GetDrugs()
        {
            var drugs = await _drug.GetDrugs();


            return Ok(new { drugs, message = "Drugs Fetched" });

        }

        [HttpPost("CreateDrug")]
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

        [HttpPost("UpdateDrug")]
        public async Task<IActionResult> UpdateDrug(DrugDtoForUpdate drug)
        {
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }        


            var drugToUpdate = _mapper.Map<Drug>(drug);

            var res = await _drug.UpdateDrug(drugToUpdate);
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
        [HttpPatch]
        public async Task<IActionResult> UpdateDrugQuantity( string Id, JsonPatchDocument<DrugDtoForUpdate> DrugForPatch)
        {

            var drug = await _drug.GetDrug(Id);
            
            if (drug == null || DrugForPatch == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            
           //then we patch
            await _drug.UpdateDrug(drug, DrugForPatch);

            return Ok(new
            {
                DrugForPatch,
                message = "Drug Quantity updated successfully"
            });
        }

        [HttpPost("DeleteDrug")]
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
