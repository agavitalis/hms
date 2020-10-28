using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.Models;
using HMS.Areas.Pharmacy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly IDrug _drugRepository;

        public DrugController(IDrug drugRepository)
        {
            _drugRepository = drugRepository;
        }

        [HttpGet]
        [Route("GetAllDrugs")]
        public async Task<IEnumerable<Drug>> GetAllDrugsAsync()
        {
            return await _drugRepository.GetAllDrugsAsync();
        }

        [HttpGet]
        [Route("GetDrugById")]
        public async Task<IActionResult> GetDrugById(string Id)
        {
            var drug = await _drugRepository.GetDrugByIdAsync(Id);
            if (drug != null)
            {
                return Ok(new { drug });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Drug Id"
                });
            }
        }

        [HttpPost]
        [Route("CreateDrug")]
        public async Task<IActionResult> CreateDrug([FromBody] CreateDrugViewModel labTestVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugRepository.CreateDrugAsync(labTestVM))
                {
                    return Ok(new
                    {
                        message = "Drug successfully created"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to create drug"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("EditDrug")]
        public async Task<IActionResult> Edit([FromBody] EditDrugViewModel labTestVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugRepository.EditDrugAsync(labTestVM))
                {
                    return Ok(new
                    {
                        message = "Drug Updated Successfully"
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(new {message = "Please FIll all fields" });
            }
        }

        [Route("GetDrugsCount")]
        [HttpGet]
        public async Task<Int64> GetDrugsCount()
        {
            return await _drugRepository.TotalNumber();
        }
        [Route("DeleteDrug")]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _drugRepository.DeleteDrugAsync(Id))
            {
                return Ok(new { message = "Drug deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete drug" });
            }
        }

        [Route("FindDrugByName")]
        [HttpGet]
        public async Task<IEnumerable<Drug>> Find(string name)
        {
            return await _drugRepository.FindByNameAsync(name);
        }
    }
}
