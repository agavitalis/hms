using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Appointments")]
    [ApiController]
    public class DoctorAppointmentController : Controller
    {
        private readonly IUser _userRepo;
        private readonly IDoctorAppointment _appointment;
        public DoctorAppointmentController(IDoctorAppointment appointment, IUser userRepo)
        {
            _appointment = appointment;
            _userRepo = userRepo;
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
        public async Task<IActionResult> RejectAnAppointment(string AppointmentId)
        {

            //check if the schedule even exist and not yet booked
            var appointment = await _appointment.GetDoctorAppointment(AppointmentId);

            if (appointment == null)
            {
                return BadRequest(new { response = 401, message = "Invalid Parameters passed" });
            }

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