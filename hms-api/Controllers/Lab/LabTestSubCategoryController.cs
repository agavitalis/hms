﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Lab.LabTestSubCategoryViewModel;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabTestSubCategoryController : Controller
    {
        private readonly ILabTestSubCategory _LabTestSubCategoryRepository;
        private readonly ILabTestInLabTestSubCategory _LabTestInLabTestSubCategory;

        public LabTestSubCategoryController(ILabTestSubCategory LabTestSubCategoryRepository, ILabTestInLabTestSubCategory LabTestInLabTestSubCategory)
        {
            _LabTestSubCategoryRepository = LabTestSubCategoryRepository;
            _LabTestInLabTestSubCategory = LabTestInLabTestSubCategory;

        }

        [HttpGet]
        [Route("GetLabTestAllSubCategories")]
        public async Task<IEnumerable<LabTestSubCategory>> GetAllSubCategoriesAsync()
        {
            return await _LabTestSubCategoryRepository.GetAllLabTestSubCategoryAsync();
        }

        [HttpGet]
        [Route("GetLabTestSubCategoryById")]
        public async Task<IActionResult> GetSubCategoryById(string Id)
        {
            var cat = await _LabTestSubCategoryRepository.GetLabTestSubCategoryByIdAsync(Id);
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
        [Route("CreateLabTestSubCategory")]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateLabTestSubCategoryViewModel subCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestSubCategoryRepository.CreateLabTestSubCategoryAsync(subCategoryVM))
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
        [Route("EditLabTestSubCategory")]
        public async Task<IActionResult> EditSubCategory([FromBody] EditLabTestSubCategoryViewModel subCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestSubCategoryRepository.EditLabTestSubCategoryAsync(subCategoryVM))
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

        [Route("GetLabTestSubCategoryTotalNumber")]
        [HttpGet]
        public async Task<Int64> Totalnumber()
        {
            return await _LabTestSubCategoryRepository.Totalnumber();
        }

        [Route("DeleteLabTestSubCategory")]
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _LabTestSubCategoryRepository.DeleteLabTestSubCategoryAsync(Id))
            {
                return Ok(new { message = "SubCategory deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete SubCategory" });
            }
        }

        [Route("FindLabTestSubCategoryByName")]
        [HttpGet]
        public async Task<IEnumerable<LabTestSubCategory>> FindSubCategoryByName(string name)
        {
            return await _LabTestSubCategoryRepository.FindByNameAsync(name);
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
