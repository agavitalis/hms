﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HMS.Areas.Admin.Controllers
{
   
    [Route("api/Admin", Name ="Admin - Manage Appointment") ]
    [ApiController]
    public class AppointmentController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUser _userRepo;
        private readonly IAppointment _appointmentRepo;
        private readonly IDoctorClerking _clerking;


        public AppointmentController(IMapper mapper, IAppointment appointment, IUser userRepo, IDoctorClerking clerking)
        {
            _userRepo = userRepo;
            _appointmentRepo = appointment;
            _mapper = mapper;
            _clerking = clerking;
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
                var doctorPatientAssignment = _mapper.Map<MyPatient>(appointment);

                var res = await _appointmentRepo.BookAppointment(doctorAppointment);

                if (!res)
                {
                    return BadRequest(new { message = "failed to book appointment" });
                }

                else
                {
                    var result = await _appointmentRepo.AssignDoctorToPatient(doctorPatientAssignment);
                    if (result)
                    {
                        return Ok(new { message = "Appointment Successfully booked" });
                    }
                    return BadRequest(new { message = "failed to assign patient to doctor" });
                }  
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
            var doctorPatientAssignment = _mapper.Map<MyPatient>(Appointment);
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
                {
                    return BadRequest(new { message = "failed to book appointment" });
                }
                else
                {
                    var result = await _appointmentRepo.AssignDoctorToPatient(doctorPatientAssignment);
                    if (result)
                    {
                        return Ok(new { message = "Appointment Successfully reassigned" });
                    }
                    return BadRequest(new { message = "failed to assign patient to doctor" });
                   
                }  
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

        [Route("DeleteAppointment")]
        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(DeleteAppointmentDto Appointment)
        {
            //check if this guy has a profile already
            var appointment = await _appointmentRepo.GetAppointment(Appointment.AppointmentId);

            if (appointment == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Appointment Id" });
            }

            var clerking = await _clerking.GetDoctorClerkingByAppointment(Appointment.AppointmentId);

            if (clerking != null)
            {
                return BadRequest(new { response = 301, message = "This Appointment Has An Associated Clerking And Cannot Be Deleted" });
            }

            await _appointmentRepo.DeleteAppointment(appointment);

            return Ok(new { message = "Appointment Successfully Deleted" });
        }

    }

}