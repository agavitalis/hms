using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HMS.Areas.Admin.Controllers
{
   
    [Route("api/Admin", Name ="Admin- Manage Appointment") ]
    [ApiController]
    public class AppointmentController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUser _userRepo;
        private readonly IAppointment _appointmentRepo;


        public AppointmentController(IMapper mapper, IAppointment appointment, IUser userRepo)
        {
            _userRepo = userRepo;
            _appointmentRepo = appointment;
            _mapper = mapper;
        }

        [Route("GetDoctorAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var doctorsAppointments = await _appointmentRepo.GetDoctorsAppointment();
            return Ok(new { doctorsAppointments, message = "List of Doctors Appointments" });
        }


        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentDto appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByIdAsync(appointment.PatientId);
            var doctor = await _userRepo.GetUserByIdAsync(appointment.DoctorId);
            // Validate patient is not null---has no profile yet
            if (patient != null && doctor != null)
            {
                //if its avaliable now book it
                var doctorAppointment = _mapper.Map<Appointment>(appointment);

                var res = await _appointmentRepo.BookAppointment(doctorAppointment);

                if (!res)
                    return BadRequest(new { message = "failed to book appointment" });
                else
                    return Ok(new { message = "Appointment Successfully booked" });
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

        [Route("ReassignAppointment")]
        [HttpPost]
        public async Task<IActionResult> ReassignAppointment(ReassignAppointmentDto Appointment)
        {
            //check if this guy has a profile already
            var appointment = await _appointmentRepo.GetAppointment(Appointment.AppointmentId);
            var doctor = await _userRepo.GetUserByIdAsync(Appointment.DoctorId);
            // Validate patient is not null---has no profile yet
            if (appointment != null && doctor != null)
            {
                //if its avaliable now book it
                var doctorAppointment = _mapper.Map<Appointment>(appointment);
                doctorAppointment.DoctorId = Appointment.DoctorId;
                var res = await _appointmentRepo.UpdateAppointment(doctorAppointment);

                if (!res)
                    return BadRequest(new { message = "failed to book appointment" });
                else
                    return Ok(new { message = "Appointment Successfully reassigned" });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Appointment Id or Doctor Id Supplied"
                });
            }
        }

    }

}