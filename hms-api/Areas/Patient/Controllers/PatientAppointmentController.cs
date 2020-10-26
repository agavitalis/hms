using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient")]
    [ApiController]
    public class PatientAppointmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PatientAppointmentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel appointment)
        {
           
            //if its avaliable now book it
            var doctorAppointment = new DoctorAppointment(){

                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                AppointmentTitle = appointment.AppointmentTitle,
                ReasonForAppointment = appointment.ReasonForAppointment,

                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId
             };

            _applicationDbContext.DoctorAppointments.Add(doctorAppointment);
           
            await _applicationDbContext.SaveChangesAsync();


            return Ok(new
            { 
                message = "Appointment Successfully booked"
            });
           
        }

        [Route("ViewAllAppointments")]
        [HttpGet]
        public async Task<IActionResult> ViewMyAppointments(string PatientId)
        {
            //get all my appointments in this system
            var appointments = await _applicationDbContext.DoctorAppointments
                .Where(s => s.PatientId == PatientId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    appointment => appointment.DoctorId,
                    applicationUser => applicationUser.Id,
                    (appointment, applicationUser) => new { appointment, applicationUser }
                )
                .Join(
                    _applicationDbContext.DoctorProfiles,
                    applicationUser => applicationUser.applicationUser.Id,
                    doctorProfile => doctorProfile.DoctorId,
                    (applicationUser, doctorProfile) => new { applicationUser, doctorProfile }
                )
                .ToListAsync();


            return Ok(new
            {
                appointments,
                message = "Patient Appointments"
            });

        }

        [Route("GetAnAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewAnAppointment(string AppointmentId)
        {
            //get an appointment using appointment ID
            var appointment = await _applicationDbContext.DoctorAppointments
                .Where(s => s.Id == AppointmentId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    appointment => appointment.DoctorId,
                    applicationUser => applicationUser.Id,
                    (appointment, applicationUser) => new { appointment, applicationUser }
                )
                .Join(
                    _applicationDbContext.DoctorProfiles,
                    applicationUser => applicationUser.applicationUser.Id,
                    doctorProfile => doctorProfile.DoctorId,
                    (applicationUser, doctorProfile) => new { applicationUser, doctorProfile }
                )
                .ToListAsync();


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

            //check if the schedule even exist and not yet book
            var appointment = await _applicationDbContext.DoctorAppointments
                .Where(s => s.Id == AppointmentId).FirstOrDefaultAsync();

            if (appointment == null)
            {
                return BadRequest(new
                {
                    response = 401,
                    message = "Invalid Parameters passed"
                });
            }
            else
            {
                appointment.IsCanceled = true;
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Appointment Cancelled Successfully"
                });
            }



        }
    }
}