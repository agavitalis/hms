using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor- Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
      
        private readonly IAppointment _appointment;
     
        public DashboardController(IAppointment appointment)
        {
           
            _appointment = appointment;
         
        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
           
            var pendingAppoinmentsCount = await _appointment.GetDoctorsPendingAppointmentsCount();
          

            return Ok(new
            {
              
                pendingAppoinmentsCount,
              
                message = "Doctor Dashboard Counts"
            });
        }

    }
}
