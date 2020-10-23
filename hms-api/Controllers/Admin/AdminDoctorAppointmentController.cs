using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Doctor;
using HMS.Services.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.ViewModels.Patient.AppointmentViewModel;

namespace HMS.Controllers.Admin
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminDoctorAppointmentController : Controller
    {
        private readonly IAdmin adminRepo;
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminDoctorAppointmentController(IAdmin adminRepo, ApplicationDbContext applicationDbContext)
        {
            this.adminRepo = adminRepo;
            this._applicationDbContext = applicationDbContext;
        }

        [Route("GetDoctorAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var result = adminRepo.GetDoctorsPatientAppointment();

            return Ok(new { result, message = "success" });
           
        }

        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(AdminBookAppointmentViewModel appointment)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Email == appointment.PatientEmail);

            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //if its avaliable now book it
                var doctorAppointment = new DoctorAppointment()
                {

                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentTitle = appointment.AppointmentTitle,
                    ReasonForAppointment = appointment.ReasonForAppointment,

                    PatientId = Patient.Id,
                    DoctorId = appointment.DoctorId
                };

                _applicationDbContext.DoctorAppointments.Add(doctorAppointment);
                await _applicationDbContext.SaveChangesAsync();

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
                    message = "Invalid Patient Email Supplied"
                });
            }

        }


    }
}
