using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/Admin")]
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
            var result = await _appointmentRepo.GetDoctorsAppointment();

            return Ok(new { result, message = "success" });

        }


        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentDto appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByEmailAsync(appointment.PatientEmail);

            // Validate patient is not null---has no profile yet
            if (patient != null)
            {
                //if its avaliable now book it
                appointment.PatientId = patient.Id;
                var doctorAppointment = _mapper.Map<DoctorAppointment>(appointment);

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
                    message = "Invalid Patient Email Supplied"
                });
            }
        }

    }
}