using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/Lab")]
    [ApiController]
    public class TestCategoryController : ControllerBase
    {
        private readonly ILabTestCategory _LabTestCategoryRepository;
        private readonly ILabTestInLabTestCategory _LabTestInLabTestCategoryRepository;

        public TestCategoryController(ILabTestCategory LabTestCategoryRepository, ILabTestInLabTestCategory LabTestInLabTestCategoryRepository)
        {
            _LabTestCategoryRepository = LabTestCategoryRepository;
            _LabTestInLabTestCategoryRepository = LabTestInLabTestCategoryRepository;
        }

        [HttpGet]
        [Route("GetAllLabTestCategories")]
        public async Task<IEnumerable<LabTestCategory>> GetAllCategoriessAsync()
        {
            return await _LabTestCategoryRepository.GetAllCategoriesAsync();
        }

        [HttpGet]
        [Route("GetLabTestCategoryById")]
        public async Task<IActionResult> GetCategoryById(string Id)
        {
            var LabTestCategory = await _LabTestCategoryRepository.GetCategoryByIdAsync(Id);
            if (LabTestCategory != null)
            {
                return Ok(new { LabTestCategory });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Lab Test Category Id"
                });
            }
        }

        [HttpPost]
        [Route("CreateLabTestCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateLabTestCategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestCategoryRepository.CreateCategoryAsync(categoryVM))
                {
                    return Ok(new
                    {
                        message = "Lab Test Category Created Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Create Lab Test Category"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("EditLabTestCategory")]
        public async Task<IActionResult> EditCategory([FromBody] EditLabTestCategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestCategoryRepository.EditCategoryAsync(categoryVM))
                {
                    return Ok(new
                    {
                        message = "Lab Test Category Updated Successfully"
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

        [Route("GetLabTestCategoryTotalNumber")]
        [HttpGet]
        public async Task<Int64> GetCategoryTotalNumber()
        {
            return await _LabTestCategoryRepository.TotalNumber();
        }

        [Route("DeleteLabTestCategory")]
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _LabTestCategoryRepository.DeleteCategoryAsync(Id))
            {
                return Ok(new { message = "Lab Test Category deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete Lab Test category" });
            }
        }

        [Route("FindCategoryByName")]
        [HttpGet]
        public async Task<IEnumerable<LabTestCategory>> FindCategoryByName(string name)
        {
            return await _LabTestCategoryRepository.FindByNameAsync(name);
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
