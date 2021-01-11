using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient", Name = "Patient- Dashboard")]
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
        public async Task<IActionResult> GetSystemCount(string patientId)
        {

            var pendingAppoinmentsCount = await _appointment.GetPatientPendingAppointmentsCount(patientId);
            var completedAppoinmentsCount = await _appointment.GetPatientCompletedAppointmentsCount(patientId);

            var pendingConsultationsCount = await _consultation.GetPatientPendingConsultationCount(patientId);
            var completedConsultationCount = await _consultation.GetPatientCompletedConsultationCount(patientId);

            var myDoctorsCount = await _myPatient.GetMyDoctorCountAsync(patientId);



            return Ok(new
            {
               
                pendingAppoinmentsCount,
                completedAppoinmentsCount,
                pendingConsultationsCount,
                completedConsultationCount,
                myDoctorsCount,
                message = "Patient Dashboard Counts"
            });
        }

    }
}
