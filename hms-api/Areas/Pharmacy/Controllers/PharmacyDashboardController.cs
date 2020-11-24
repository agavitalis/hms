using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;

using Microsoft.AspNetCore.Mvc;


namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy- Dashboard")]
    [ApiController]
    public class PharmacyDashboardController : Controller
    {
        
        private readonly IDrug _drugRepository;

        public PharmacyDashboardController(IDrug drugRepository)
        {
          
            _drugRepository = drugRepository;
        }

        [Route("SystemSummary")]
        [HttpGet]
        public async Task<IActionResult> GetDashboardCountAsync()
        {
            if (ModelState.IsValid)
            {
                var drugCount = await _drugRepository.GetDrugCount();
             

                return Ok(new
                {
                    drugCount,
                  
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
