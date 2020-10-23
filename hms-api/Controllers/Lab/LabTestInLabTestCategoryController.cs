using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Lab.LabTestInLabTestCategoryViewModel;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabTestInLabTestCategoryController : Controller
    {
        private readonly ILabTestInLabTestCategory _LabTestInLabTestCategoryRepository;

        public LabTestInLabTestCategoryController(ILabTestInLabTestCategory LabTestInLabTestCategoryRepository)
        {
            _LabTestInLabTestCategoryRepository = LabTestInLabTestCategoryRepository;
        }

        [HttpGet]
        [Route("GetAllLabTestInLabTestCategories")]
        public async Task<IEnumerable<LabTestInLabTestCategory>> GetAllLabTestInLabTestCategoryAsync()
        {

            return await _LabTestInLabTestCategoryRepository.GetAllLabTestInLabTestCategoriesAsync();
        }

        [HttpGet]
        [Route("GetLabTestInLabTestCategoryById")]
        public async Task<IActionResult> GetLabTestInLabTestCategoryById(string Id)
        {
            var categories = await _LabTestInLabTestCategoryRepository.GetLabTestInLabTestCategoryByIdAsync(Id);

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
        [Route("GetLabTestInLabTestCategoryByLabTestCategoryId")]
        public async Task<IActionResult> GetLabTestInLabTestCategoryByLabTestCategoryId(string LabTestCategoryId)
        {
            var labTests = await _LabTestInLabTestCategoryRepository.GetLabTestInLabTestCategoryByLabTestCategoryIdAsync(LabTestCategoryId);

            if (labTests != null)
            {
                return Ok(new { labTests });
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
        [Route("AddLabTestToALabTestCategory")]
        public async Task<IActionResult> AddLabTestToALabTestCategory([FromBody] CreateLabTestInLabTestCategoryViewModel LabTest)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestInLabTestCategoryRepository.CreateLabTestInLabTestCategoryAsync(LabTest))
                {
                    return Ok(new
                    {
                        message = "LabTest successfully added to this Category"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert LabTest to this Category"
                    });
                }
            }
            return BadRequest(new { message = "Please fill all fields" });
        }
    }
}
