using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant", Name = "Accountant - Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
       
        private readonly IUser _user;
        private readonly IServices _services;
        private readonly IDrug _drug;

        public DashboardController( IUser user, IServices services, IDrug drug)
        {
          
            _user = user;
            _services = services;
            _drug = drug;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount(string accountantId)
        {
          
            var userCount = await _user.GetUserCount();
            var serviceRequestCount = await _services.GetServiceRequestCount();
            var drugCount = await _drug.GetDrugCount();

            return Ok(new
            {
                userCount,
                serviceRequestCount,
                drugCount,
                message = "Accountant Dashboard Counts"
            });
        }
    }
}
