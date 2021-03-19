using System.Threading.Tasks;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient", Name = "Patient - Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {

        private readonly IPatientAppointment _appointment;
        private readonly IPatientConsultation _consultation;
        private readonly IMyPatient _myPatient;

        public DashboardController(IPatientAppointment appointment, IMyPatient myPatient, IPatientConsultation consultation)
        {

            _appointment = appointment;
            _consultation = consultation;
            _myPatient = myPatient;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount(string patientId)
        {

            var pendingAppoinmentsCount = await _appointment.GetPendingAppointmentsCount(patientId);
            var completedAppoinmentsCount = await _appointment.GetCompletedAppointmentsCount(patientId);
            var canceledAppointmentsCount = await _appointment.GetCanceledAppointmentsCount(patientId);
            var pendingConsultationsCount = await _consultation.GetPendingConsultationsCount(patientId);
            var completedConsultationCount = await _consultation.GetCompletedConsultationsCount(patientId);
            var canceledConsultationCount = await _consultation.GetCanceledConsultationsCount(patientId);
            var myDoctorsCount = await _myPatient.GetMyDoctorCountAsync(patientId);



            return Ok(new
            {
               
                pendingAppoinmentsCount,
                completedAppoinmentsCount,
                canceledAppointmentsCount,
                pendingConsultationsCount,
                completedConsultationCount,
                canceledConsultationCount,
                myDoctorsCount,
                message = "Patient Dashboard Counts"
            });
        }
    }
}
