using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Lab.LabTestInLabTestSubCategoryViewModel;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabTestInLabTestSubCategoryController : Controller
    {
        private readonly ILabTestInLabTestSubCategory _LabTestInLabTestSubCategory;

        public LabTestInLabTestSubCategoryController(ILabTestInLabTestSubCategory LabTestInLabTestSubCategory)
        {
            _LabTestInLabTestSubCategory = LabTestInLabTestSubCategory;
        }

        [HttpGet]
        [Route("GetAllLabTestInLabTestSubCategories")]
        public async Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestSubCategoriessAsync()
        {
            return await _LabTestInLabTestSubCategory.GetAllLabTestInLabTestSubCategoriesAsync();
        }

        [HttpGet]
        [Route("GetLabTestInLabTestSubCategoryById")]
        public async Task<IActionResult> GetLabTestInLabTestSubCategoryById(string Id)
        {
            var cat = await _LabTestInLabTestSubCategory.GetLabTestInLabTestSubCategoryIdAsync(Id);
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
        [Route("GetLabTestInLabTestSubCategoryBySubCategoryId")]
        public async Task<IActionResult> GetLabTestInLabTestSubCategoryBySubCAtegoryId(string SubCategoryId)
        {
            var cat = await _LabTestInLabTestSubCategory.GetAllLabTestInLabTestSubCategoriesBySubCategoryIdAsync(SubCategoryId);
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
        [Route("AddLabTestToALabTestSubCategory")]
        public async Task<IActionResult> AddLabTestToALabTestSubCategory([FromBody] CreateLabTestInLabTestSubCategoryViewModel createLabTestSubCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestInLabTestSubCategory.CreateLabTestInLabTestSubCategoryAsync(createLabTestSubCategoryVM))
                {
                    return Ok(new
                    {
                        message = "LabTest successfully added to this SubCategory"
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
