using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class DrugSubCategoryController : Controller
    {

        private readonly IDrugSubCategory _drugSubCategoryRepository;

        public DrugSubCategoryController(IDrugSubCategory drugSubCategoryRepository)
        {
            _drugSubCategoryRepository = drugSubCategoryRepository;
        }

        [HttpGet]
        [Route("GetDrugAllSubCategories")]
        public async Task<IEnumerable<DrugSubCategory>> GetAllSubCategoriesAsync()
        {
            return await _drugSubCategoryRepository.GetAllDrugSubCategoryAsync();
        }

        [HttpGet]
        [Route("GetDrugSubCategoryById")]
        public async Task<IActionResult> GetSubCategoryById(string Id)
        {
            var cat = await _drugSubCategoryRepository.GetDrugSubCategoryByIdAsync(Id);
            if (cat != null)
            {
                return Ok(new { cat });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Id"
                });
            }
        }

        [HttpPost]
        [Route("CreateDrugSubCategory")]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateDrugSubCategoryViewModel subCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugSubCategoryRepository.CreateDrugSubCategoryAsync(subCategoryVM))
                {
                    return Ok(new
                    {
                        message = "Subcategory Created Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("EditDrugSubCategory")]
        public async Task<IActionResult> EditSubCategory([FromBody] EditDrugSubCategoryViewModel subCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugSubCategoryRepository.EditDrugSubCategoryAsync(subCategoryVM))
                {
                    return Ok(new
                    {
                        message = "SubCategory Updated Successfully"
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(new { message = "Please fill all fields" });
            }
        }

        [Route("GetDrugSubCategoryTotalNumber")]
        [HttpGet]
        public async Task<Int64> Totalnumber()
        {
            return await _drugSubCategoryRepository.Totalnumber();
        }

        [Route("DeleteDrugSubCategory")]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _drugSubCategoryRepository.DeleteDrugSubCategoryAsync(Id))
            {
                return Ok(new { message = "SubCategory deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete SubCategory" });
            }
        }

        [Route("FindDrugSubCategoryByName")]
        [HttpGet]
        public async Task<IEnumerable<DrugSubCategory>> FindSubCategoryByName(string name)
        {
            return await _drugSubCategoryRepository.FindByNameAsync(name);
        }

    }
}
