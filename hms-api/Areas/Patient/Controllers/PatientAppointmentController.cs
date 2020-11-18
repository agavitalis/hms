using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient", Name = "Patient- Manage Doctor Appointments")]
    [ApiController]
    public class PatientAppointmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUser _userRepo;
        private readonly IPatientAppointment _appointment;
        public PatientAppointmentController(ApplicationDbContext applicationDbContext, IUser userRepo, IPatientAppointment appointment)
        {
            _applicationDbContext = applicationDbContext;
            _appointment = appointment;
            _userRepo = userRepo;

        }

        [Route("GetPendingAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPendingAppoinmentsCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetPendingAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCompletedAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsUnattendedToCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetCompletedAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCanceledAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetCanceledAppointmentsCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetCanceledAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetPendingAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPendingAppointments(string patientId)
        {
            if (patientId == null)
            {
                return BadRequest(new { message = "Invalid Patient Id or Doctor Id Supplied" });
            }
            
            var appointments = await _appointment.GetPendingAppointments(patientId);

            
            return Ok(new
            {
                appointments,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByIdAsync(appointment.PatientId);
            var doctor = await _userRepo.GetUserByIdAsync(appointment.DoctorId);
            if (patient != null && doctor != null)
            {
                _appointment.BookAppointment(appointment);
                //if its avaliable now book it
                

                return Ok(new
                {
                    message = "Appointment Successfully booked"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id or Doctor Id Supplied"
                });
            }
        }

        [Route("ViewAllAppointments")]
        [HttpGet]
        public async Task<IActionResult> ViewMyAppointments(string PatientId)
        {
            var patient = await _userRepo.GetUserByIdAsync(PatientId);

            if (patient != null)
            {
                
               var appointments = _appointment.GetPatientAppointments(PatientId);
                //if its avaliable now book it


                return Ok(new
                {
                    appointments,
                    message = "Patient Appointments"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id"
                });
            }


            

        }

        [Route("GetAnAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewAnAppointment(string AppointmentId)
        {
            //get an appointment using appointment ID
            var appointment = await _appointment.GetPatientAppointment(AppointmentId);
                


            return Ok(new
            {
                appointment,
                message = "Complete Appointment Details Appointments"
            });

        }

        [Route("CancelAnAppointment")]
        [HttpPatch]
        public async Task<IActionResult> CancelAnAppointment(string AppointmentId)
        {
            var response = await _appointment.CancelAppointment(AppointmentId);

            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Appointment succesfully Cancelled"

                });
            }
            else if (response == 1)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Appointment Id"
                });
            }
            else if (response == 2)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Appointment Has Been Canceled By Doctor"
                });
            }
            else if (response == 3)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Appointment Has Expired"
                });
            }
            else if (response == 4)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Appointment Was Rejected"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }
        }
    }
}