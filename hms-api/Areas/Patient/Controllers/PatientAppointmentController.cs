using System;
using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient", Name = "Patient - Manage Appointments")]
    [ApiController]
    public class PatientAppointmentController : Controller
    {
    
        private readonly IUser _userRepo;
        private readonly IPatientAppointment _appointment;
        private readonly IPatientProfile _patient;
        public PatientAppointmentController(IUser userRepo, IPatientAppointment appointment, IPatientProfile patient)
        {
            
            _appointment = appointment;
            _patient = patient;
            _userRepo = userRepo;

        }


        [Route("GetCompletedAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetCompletedAppointments([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }
            var appointments = _appointment.GetCompletedAppointments(paginationParameter, PatientId);

            var paginationDetails = new
            {
                appointments.TotalCount,
                appointments.PageSize,
                appointments.CurrentPage,
                appointments.TotalPages,
                appointments.HasNext,
                appointments.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                appointments,
                paginationDetails,
                message = "Appointments returned"
            });
        }

        [Route("GetPendingAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPendingAppointments([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }
            var appointments = _appointment.GetPendingAppointments(paginationParameter, PatientId);

            var paginationDetails = new
            {
                appointments.TotalCount,
                appointments.PageSize,
                appointments.CurrentPage,
                appointments.TotalPages,
                appointments.HasNext,
                appointments.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                appointments,
                paginationDetails,
                message = "Appointments returned"
            });
        }


        [Route("GetCanceledAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetCanceledAppointments([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }

            var appointments = _appointment.GetCanceledAppointments(paginationParameter, PatientId);

            var paginationDetails = new
            {
                appointments.TotalCount,
                appointments.PageSize,
                appointments.CurrentPage,
                appointments.TotalPages,
                appointments.HasNext,
                appointments.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                appointments,
                paginationDetails,
                message = "Appointments returned"
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
                var myPatient = new MyPatient();

                myPatient = new MyPatient()
                {
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    DateCreated = DateTime.Now
                };
                var result = await _appointment.AssignDoctorToPatient(myPatient);
                await _appointment.BookAppointment(appointment);
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

        [Route("GetAnAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewAnAppointment(string AppointmentId)
        {
            //get an appointment using appointment ID
            var appointment = await _appointment.GetPatientAppointment(AppointmentId);
                


            return Ok(new
            {
                appointment,
                message = "Appointment Returned"
            });

        }

        [Route("CancelAnAppointment")]
        [HttpPost]
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