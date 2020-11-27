using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin- Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IPatientProfile _patient;
        private readonly IDoctorProfile _doctor;
        private readonly IAppointment _appointment;
        private readonly IUser _user;
        private readonly IServices _services;
        private readonly IDrug _drug;




        public DashboardController(IPatientProfile patient, IDoctorProfile doctor, IAppointment appointment, IUser user, IServices services, IDrug drug)
        {
            _patient = patient;
            _doctor = doctor;
            _appointment = appointment;
            _user = user;
            _services = services;
            _drug = drug;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
            var patientCount = await _patient.GetPatientCountAsync();
            var doctorCount = await _doctor.GetDoctorCountAsync();
            var pendingAppoinmentsCount = await _appointment.GetDoctorsPendingAppointmentsCount();
            var userCount = await _user.GetUserCount();
            var serviceRequestCount = await _services.GetServiceRequestCount();
            var drugCount = await _drug.GetDrugCount();

            return Ok(new
            {
                patientCount,
                doctorCount,
                pendingAppoinmentsCount,
                userCount,
                serviceRequestCount,
                drugCount,
                message = "Admin Dashboard Counts"
            });
        }

    }
}
