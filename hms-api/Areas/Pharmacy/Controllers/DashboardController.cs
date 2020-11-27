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
    public class DashboardController : Controller
    {
    
        private readonly IDrug _drug;


        public DashboardController(IDrug drug)
        {
            
            _drug = drug;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
           
            var drugCount = await _drug.GetDrugCount();

            return Ok(new
            {
                drugCount,
                message = "Pharmacy Dashboard Counts"
            });
        }
    }
}
