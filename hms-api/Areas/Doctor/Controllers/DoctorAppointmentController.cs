using System.Threading.Tasks;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Appointments")]
    [ApiController]
    public class DoctorAppointmentController : Controller
    {
        private readonly IUser _userRepo;
        private readonly IDoctorAppointment _appointment;
        private readonly IDoctorProfile _doctor;
        public DoctorAppointmentController(IDoctorAppointment appointment, IDoctorProfile doctor, IUser userRepo)
        {
            _appointment = appointment;
            _userRepo = userRepo;
            _doctor = doctor;
        }
        [Route("ViewAllAppointments")]
        [HttpGet]
        public async Task<IActionResult> ViewMyAppointments(string DoctorId)
        {
            var doctor = await _userRepo.GetUserByIdAsync(DoctorId);

            if (doctor != null)
            {

                var appointments = await _appointment.GetDoctorAppointments(DoctorId);
                //if its avaliable now book it


                return Ok(new
                {
                    appointments,
                    message = "Doctor Appointments"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Doctor Id"
                });
            }


        }

        [Route("GetDoctorAppointmentsPending")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAppointmentsPending(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var doctor = await _doctor.GetDoctorAsync(DoctorId);

            if (doctor == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Doctor Id" });
            }
            
            var appointments = _appointment.GetAppointmentsPending(DoctorId, paginationParameter);

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
                message = "Appointments Fetched"
            });
        }

        [Route("GetDoctorAppointmentsCompleted")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAppointmentsCompleted(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var doctor = await _doctor.GetDoctorAsync(DoctorId);

            if (doctor == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Doctor Id" });
            }

            var appointments = _appointment.GetAppointmentsCompleted(DoctorId, paginationParameter);

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
                message = "Appointments Fetched"
            });
        }

        [Route("GetDoctorAppointmentsAccepted")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAppointmentsAccepted(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var doctor = await _doctor.GetDoctorAsync(DoctorId);

            if (doctor == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Doctor Id" });
            }

            var appointments = _appointment.GetAppointmentsAccepted(DoctorId, paginationParameter);

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
                message = "Appointments Fetched"
            });
        }

        [Route("GetAnAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewAnAppointment(string AppointmentId)
        {
            //get an appointment using appointment ID
            var appointment = await _appointment.GetDoctorAppointment(AppointmentId);

            return Ok(new
            {
                appointment,
                message = "Complete Appointment Details Appointments"
            });
        }

        [Route("AcceptAnAppointment")]
        [HttpPost]
        public async Task<IActionResult> AcceptAnAppointment(string AppointmentId)
        {

            //check if the schedule even exist and not yet booked
            var appointment = await _appointment.GetDoctorAppointment(AppointmentId);

            if (appointment == null)
            {
                return BadRequest(new { response = 401, message = "Invalid Parameters passed" });
            }

            var response = await _appointment.AcceptAppointment(appointment);

            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Appointment succesfully Accepted"

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
                    message = "Appointment Has Been Completed"
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
            
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }
        }

        [Route("RejectAnAppointment")]
        [HttpPost]
        public async Task<IActionResult> RejectAnAppointment(DoctorAppointmentDtoForReject Appointment)
        {

            //check if the schedule even exist and not yet booked
            var appointment = await _appointment.GetDoctorAppointment(Appointment.AppointmentId);

            if (appointment == null)
            {
                return BadRequest(new { response = 401, message = "Invalid Parameters passed" });
            }

            appointment.ReasonForRejection = Appointment.RejectionNote;

            var response = await _appointment.RejectAppointment(appointment);

            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Appointment succesfully Rejected"

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
                    message = "Appointment Has Been Completed"
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

            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }

        }

        //only happens when an appointment have ealier been accepted
        [Route("CancelAnAppointment")]
        [HttpPost]
        public async Task<IActionResult> CancelAnAppointment(string AppointmentId)
        {

            //check if the schedule even exist and not yet book

            var appointment = await _appointment.GetDoctorAppointment(AppointmentId);

            if (appointment == null)
            {
                return BadRequest(new { response = 401, message = "Invalid Parameters passed" });
            }

            var response = await _appointment.CancelAppointment(appointment);

            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Appointment succesfully Canceled"

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
                    message = "Appointment Has Been Completed"
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
                    message = "Appointment Has Been Canceled By Patient"
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