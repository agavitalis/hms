using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;

using Microsoft.AspNetCore.Mvc;


namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class PharmacyDashboardController : Controller
    {
        private readonly IDrugCategory _drugCategoryRepository;
        private readonly IDrugSubCategory _drugSubCategoryRepository;
        private readonly IDrug _drugRepository;

        public PharmacyDashboardController(IDrug drugRepository, IDrugSubCategory drugSubCategoryRepository, IDrugCategory drugCategoryRepository)
        {
            _drugCategoryRepository = drugCategoryRepository;
            _drugSubCategoryRepository = drugSubCategoryRepository;
            _drugRepository = drugRepository;
        }

        [Route("SystemSummary")]
        [HttpGet]
        public async Task<IActionResult> GetDashboardCountAsync()
        {
            if (ModelState.IsValid)
            {
                var drugCount = await _drugRepository.TotalNumber();
                var subCategoryCount = await _drugSubCategoryRepository.Totalnumber();
                var categoryCount = await _drugCategoryRepository.TotalNumber();

                return Ok(new
                {
                    drugCount,
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
