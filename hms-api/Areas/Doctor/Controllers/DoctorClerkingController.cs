using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Clerking")]
    [ApiController]
    public class DoctorClerkingController : Controller
    {
        private readonly IDoctorClerking _clerking;
        private readonly IMapper _mapper;
        private readonly IDoctorAppointment _appointment;
        private readonly IConsultation _consultation;
        private readonly IPatientProfile _patient;
        public DoctorClerkingController(IDoctorClerking clerking, IMapper mapper, IDoctorAppointment appointment, IConsultation consultation, IPatientProfile patient)
        {
            _clerking = clerking;
            _mapper = mapper;
            _appointment = appointment;
            _consultation = consultation;
            _patient = patient;
        }

        [Route("GetClerkingHistoryForPatient")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForPatient(string PatientId)
        {
            if (PatientId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var patient = await _patient.GetPatientByIdAsync(PatientId);

            if (patient == null)
            {
                return NotFound();
            }

            var clerkingHistory = await _clerking.GetDoctorClerkingByPatient(PatientId);

            return Ok(new
            {
                clerkingHistory,
                message = "Clerking History returned" 
            });
        }

        [Route("GetClerkingForAppointment")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForAppointment(string AppointmentId)
        {
            if (AppointmentId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var appoinmtent = await _appointment.GetAppointmentById(AppointmentId);

            if (appoinmtent == null)
            {
                return NotFound();
            }

            var clerking = await _clerking.GetDoctorClerkingByAppointment(AppointmentId);

            return Ok(new
            {
                clerking,
                message = "Clerking Returned"
            });
        }

        [Route("GetClerkingForConsultation")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForConsultation(string ConsultationId)
        {
            if (ConsultationId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var consultation = await _consultation.GetConsultationById(ConsultationId);

            if (consultation == null)
            {
                return NotFound();
            }

            var clerking = await _clerking.GetDoctorClerkingByConsultation(ConsultationId);

            return Ok(new
            {
                clerking,
                message = "Clerking Returned"
            });
        }

        [Route("CreatePatientClerking")]
        [HttpPost]
        public async Task<IActionResult> CreatePatientClerking(DoctorClerkingDtoForCreate clerking)
        {
            if (clerking == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var clerkingToCreate = _mapper.Map<DoctorClerking>(clerking);

            var res = await _clerking.CreateDoctorClerking(clerkingToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed to create clerking" });
            }

            return Ok(new
            {
                clerking,
                message = "Clerking created successfully"
            });
        }

        [Route("UpdatePatientClerking")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePatientClerking(string Id, string IdType, string UserId, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking)
        {

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
            if (clerking == null || Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var doctorClerking = await _clerking.GetDoctorClerkingByAppointmentOrConsultation(Id);

            if (doctorClerking == null)
            {
                doctorClerking = await _clerking.CreateDoctorClerking(Id, IdType);
            }

            //then we patch
            await _clerking.UpdateDoctorClerking(UserId, doctorClerking,  clerking);

            return Ok(new
            {
                clerking,
                message = "Clerking updated successfully"
            });
        }
    }
}
