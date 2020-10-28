﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IRegister _adminRepo;
        private readonly IUser _userRepo;
        private readonly IMapper _mapper;

        public AppointmentController(IRegister adminRepo, IUser userRepo, IMapper mapper)
        {
            _adminRepo = adminRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [Route("GetDoctorAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var result = await _adminRepo.GetDoctorsPatientAppointment();

            return Ok(new { result, message = "success" });

        }


        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(CreateBookAppointmentDto appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByEmailAsync(appointment.PatientEmail);

            // Validate patient is not null---has no profile yet
            if (patient != null)
            {
                //if its avaliable now book it
                appointment.PatientId = patient.Id;
                var doctorAppointment = _mapper.Map<DoctorAppointment>(appointment);

                var res = await _adminRepo.BookAppointment(doctorAppointment);
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