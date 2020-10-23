using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Models.Pharmacy;
using HMS.Services.Interfaces.Pharmacy;
using HMS.ViewModels.Pharmacy;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers.Pharmacy
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class DrugCategoryController : Controller
    {
        private readonly IDrugCategory _drugCategoryRepository;

        public DrugCategoryController(IDrugCategory drugCategoryRepository)
        {
            _drugCategoryRepository = drugCategoryRepository;
        }

        [HttpGet]
        [Route("GetAllDrugCategories")]
        public async Task<IEnumerable<DrugCategory>> GetAllCategoriessAsync()
        {
            return await _drugCategoryRepository.GetAllCategoriesAsync();
        }

        [HttpGet]
        [Route("GetDrugCategoryById")]
        public async Task<IActionResult> GetCategoryById(string Id)
        {
            var drugCategory = await _drugCategoryRepository.GetCategoryByIdAsync(Id);
            if (drugCategory != null)
            {
                return Ok(new { drugCategory });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Test Id"
                });
            }
        }

        [HttpPost]
        [Route("CreateDrugCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateDrugCategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugCategoryRepository.CreateCategoryAsync(categoryVM))
                {
                    return Ok(new
                    {
                        message = "Category Created Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Create Test Category"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("EditDrugCategory")]
        public async Task<IActionResult> EditCategory([FromBody] EditDrugCategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugCategoryRepository.EditCategoryAsync(categoryVM))
                {
                    return Ok(new
                    {
                        message = "Category Updated Successfully"
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

        [Route("GetDrugGategoryTotalNumber")]
        [HttpGet]
        public async Task<Int64> GetGategoryTotalNumber()
        {
            return await _drugCategoryRepository.TotalNumber();
        }

        [Route("DeleteDrugCategory")]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _drugCategoryRepository.DeleteCategoryAsync(Id))
            {
                return Ok(new { message = "Drug Category deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete drug category" });
            }
        }

        [Route("FindCategoryByName")]
        [HttpGet]
        public async Task<IEnumerable<DrugCategory>> FindCategoryByName(string name)
        {
            return await _drugCategoryRepository.FindByNameAsync(name);
        }

    }
}
