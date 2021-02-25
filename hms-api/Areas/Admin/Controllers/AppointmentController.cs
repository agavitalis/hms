using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetAppointments([FromQuery] PaginationParameter paginationParameter)
        {
            var appointments = _appointmentRepo.GetAppointmentsPagination(paginationParameter);

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

        [Route("GetDoctorAppointmentsPending")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAppointmentsPending([FromQuery] PaginationParameter paginationParameter)
        {
            var appointments = _appointmentRepo.GetAppointmentsPending(paginationParameter);

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
        public async Task<IActionResult> GetDoctorAppointmentsCompleted([FromQuery] PaginationParameter paginationParameter)
        {
            var appointments = _appointmentRepo.GetAppointmentsCompleted(paginationParameter);

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
        public async Task<IActionResult> GetDoctorAppointmentsAccepted([FromQuery] PaginationParameter paginationParameter)
        {
            var appointments = _appointmentRepo.GetAppointmentsAccepted(paginationParameter);

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

        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentDto appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByIdAsync(appointment.PatientId);
            var doctor = await _userRepo.GetUserByIdAsync(appointment.DoctorId);
            var doctorPatient = await _appointmentRepo.CheckDoctorInMyPatients(appointment.DoctorId, appointment.PatientId);
            // Validate patient is not null---has no profile yet
            if (patient != null && doctor != null)
            {
                //if its avaliable now book it
                var doctorAppointment = _mapper.Map<Appointment>(appointment);
          
                var res = await _appointmentRepo.BookAppointment(doctorAppointment);

                if (!res)
                {
                    return BadRequest(new { message = "failed to book appointment" });
                }

                else
                {
                    if (doctorPatient == null)
                    {
                        var myPatient = new MyPatient();

                        myPatient = new MyPatient()
                        {
                            DoctorId = appointment.DoctorId,
                            PatientId = appointment.PatientId,
                            DateCreated = DateTime.Now
                        };

                        var result = await _appointmentRepo.AssignDoctorToPatient(myPatient);
                        if (result)
                        {
                            return Ok(new { message = "Appointment Successfully Booked" });
                        }
                        else
                        {
                            return BadRequest(new { message = "Failed To Assign Patient To Doctor" });
                        }
                    }
                    else
                    {
                        return Ok(new { message = "Appointment Successfully Booked" });
                    }

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
            var appointment = await _appointmentRepo.GetAppointment(Appointment.AppointmentId);
            var doctor = await _userRepo.GetUserByIdAsync(Appointment.DoctorId);
            var doctorPatient = await _appointmentRepo.CheckDoctorInMyPatients(Appointment.DoctorId, appointment.PatientId);
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
                    if (doctorPatient == null)
                    {
                        var myPatient = new MyPatient();

                        myPatient = new MyPatient()
                        {
                            DoctorId = Appointment.DoctorId,
                            PatientId = appointment.PatientId,
                            DateCreated = DateTime.Now
                        };

                        var result = await _appointmentRepo.AssignDoctorToPatient(myPatient);
                        if (result)
                        {
                            return Ok(new { message = "Appointment Successfully reassigned" });
                        }
                        else
                        {
                            return BadRequest(new { message = "failed to assign patient to doctor" });
                        }
                    }
                    else
                    {
                        return Ok(new { message = "Appointment Successfully reassigned" });
                    }
                    
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