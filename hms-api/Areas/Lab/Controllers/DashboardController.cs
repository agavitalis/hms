using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/Lab", Name = "Lab Attendant - Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
     
        private readonly IServices _services;

        public DashboardController( IServices services)
        {
           
            _services = services;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
        
            var serviceRequestCount = await _services.GetServiceRequestCount();
            //var serviceRequestCount = await _services.GetServiceRequestCount();
            //var serviceRequestCount = await _services.GetServiceRequestCount();
            //var serviceRequestCount = await _services.GetServiceRequestCount();


            return Ok(new
            {
               
                serviceRequestCount,

                message = "Patient Dashboard Counts"
            });
        }

    }
}
