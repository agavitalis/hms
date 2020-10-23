using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabDashboardController : Controller
    {
        private readonly ILabTestCategory _labTestCategoryRepository;
        private readonly ILabTestSubCategory _labTestSubCategoryRepository;
        private readonly ILabTest _labTestRepository;

        public LabDashboardController(ILabTest labTestRepository, ILabTestSubCategory labTestSubCategoryRepository, ILabTestCategory labTestCategoryRepository)
        {
            _labTestCategoryRepository = labTestCategoryRepository;
            _labTestSubCategoryRepository = labTestSubCategoryRepository;
            _labTestRepository = labTestRepository;
        }

        [Route("SystemSummary")]
        [HttpGet]
        public async Task<IActionResult> GetDashboardCountAsync()
        {
            if (ModelState.IsValid)
            {
                var labTestCount = await _labTestRepository.TotalNumber();
                var subCategoryCount = await _labTestSubCategoryRepository.Totalnumber();
                var categoryCount = await _labTestCategoryRepository.TotalNumber();

                return Ok(new
                {
                    labTestCount,
                    subCategoryCount,
                    categoryCount
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Parameters"
                });
            }
        }
    }
}
