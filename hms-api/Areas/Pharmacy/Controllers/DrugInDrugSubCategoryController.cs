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
    public class DrugInDrugSubCategoryController : ControllerBase
    {
        private readonly IDrugInDrugSubCategory _drugInDrugSubCategory;

        public DrugInDrugSubCategoryController(IDrugInDrugSubCategory drugInDrugSubCategory)
        {
            _drugInDrugSubCategory = drugInDrugSubCategory;
        }

        [HttpGet]
        [Route("GetAllDrugInDrugSubCategories")]
        public async Task<IEnumerable<DrugInDrugSubCategory>> GetAllDrugSubCategoriessAsync()
        {
            return await _drugInDrugSubCategory.GetAllDrugInDrugSubCategoriesAsync();
        }

        [HttpGet]
        [Route("GetDrugInDrugSubCategoryById")]
        public async Task<IActionResult> GetDrugInDrugSubCategoryById(string Id)
        {
            var cat = await _drugInDrugSubCategory.GetDrugInDrugSubCategoryIdAsync(Id);
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

        [HttpGet]
        [Route("GetDrugInDrugSubCategoryByDrugSubCategoryId")]
        public async Task<IActionResult> GetDrugInDrugCategoryByDrugCategoryId(string SubCategoryId)
        {
            var drugs = await _drugInDrugSubCategory.GetDrugInDrugSubCategoryByDrugSubCategoryIdAsync(SubCategoryId);

            if (drugs != null)
            {
                return Ok(new { drugs });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid  Id"
                });
            }
        }

        [HttpPost]
        [Route("AddDrugToADrugSubCategory")]
        public async Task<IActionResult> AddDrugToADrugSubCategory([FromBody] CreateDrugInDrugSubCategoryViewModel createDrugSubCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _drugInDrugSubCategory.CreateDrugInDrugSubCategoryAsync(createDrugSubCategoryVM))
                {
                    return Ok(new
                    {
                        message = "Drug successfully added to this SubCategory"
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
    }
}
