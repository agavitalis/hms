using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor- Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
      
        private readonly IAppointment _appointment;
        private readonly IConsultation _consultation;
        private readonly IMyPatient _myPatient;

        public DashboardController(IAppointment appointment, IMyPatient myPatient, IConsultation consultation)
        {
           
            _appointment = appointment;
            _consultation = consultation;
            _myPatient = myPatient;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount(string doctorId)
        {
           
            var pendingAppoinmentsCount = await _appointment.GetDoctorsPendingAppointmentsCount(doctorId);
            var completedAppoinmentsCount = await _appointment.GetDoctorsCompletedAppointmentsCount(doctorId);

            var pendingConsultationsCount = await _consultation.GetDoctorsPendingConsultationCount(doctorId);
            var completedConsultationCount = await _consultation.GetDoctorsCompletedConsultationCount(doctorId);

            var myPatientsCount = await _myPatient.GetMyPatientCountAsync(doctorId);

            return Ok(new
            {
                pendingAppoinmentsCount,
                completedAppoinmentsCount,
                pendingConsultationsCount,
                completedConsultationCount,
                myPatientsCount,
                message = "Doctor Dashboard Counts"
            });
        }

    }
}
