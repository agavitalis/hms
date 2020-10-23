using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Lab.LabTestCategoryViewModel;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabTestCategoryController : Controller
    {
        private readonly ILabTestCategory _LabTestCategoryRepository;

        public LabTestCategoryController(ILabTestCategory LabTestCategoryRepository)
        {
            _LabTestCategoryRepository = LabTestCategoryRepository;
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
    }
}
