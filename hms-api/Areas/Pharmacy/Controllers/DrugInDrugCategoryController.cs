using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy- Manage Drugs Relationships")]
    [ApiController]
    public class DrugInCategoryController : Controller
    {

        private readonly IDrugInDrugCategory _drugInDrugCategoryRepository;

        public DrugInCategoryController(IDrugInDrugCategory drugInDrugCategoryRepository)
        {
            _drugInDrugCategoryRepository = drugInDrugCategoryRepository;
        }

        [HttpGet]
        [Route("GetAllDrugInDrugCategories")]
        public async Task<IEnumerable<DrugInDrugCategory>> GetAllDrugInDrugCategoryAsync()
        {
           
            return await _drugInDrugCategoryRepository.GetAllDrugInDrugCategoriesAsync();
        }

        [HttpGet]
        [Route("GetDrugInDrugCategoryById")]
        public async Task<IActionResult> GetDrugInDrugCategoryById(string Id)
        {
            var categories = await _drugInDrugCategoryRepository.GetDrugInDrugCategoryByIdAsync(Id);
        
            if (categories != null)
            {
                return Ok(new { categories });
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

        [HttpGet]
        [Route("GetDrugInDrugCategoryByDrugCategoryId")]
        public async Task<IActionResult> GetDrugInDrugCategoryByDrugCategoryId(string CategoryId)
        {
            var drugs = await _drugInDrugCategoryRepository.GetDrugInDrugCategoryByDrugCategoryIdAsync(CategoryId);

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
        [Route("AddDrugToADrugCategory")]
        public async Task<IActionResult> AddDrugToADrugCategory([FromBody] CreateDrugInDrugCategoryViewModel drug)
        {
            if (ModelState.IsValid)
            {
                if (await _drugInDrugCategoryRepository.CreateDrugInDrugCategoryAsync(drug))
                {
                    return Ok(new
                    {
                        message = "Drug successfully added to this Category"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Drug to this Category"
                    });
                }
            }
            return BadRequest(new { message = "Please fill all fields" });
        }
    }
}
