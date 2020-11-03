using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor- Manage Appointments")]
    [ApiController]
    public class DoctorAppointmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorAppointmentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }
        [Route("ViewAllAppointments")]
        [HttpGet]
        public async Task<IActionResult> ViewMyAppointments(string DoctorId)
        {
            //get all my appointments in this system with associated patient
            var appointments = await _applicationDbContext.DoctorAppointments
                .Where(s => s.DoctorId == DoctorId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    appointment => appointment.PatientId,
                    applicationUser => applicationUser.Id,
                    (appointment, applicationUser) => new { appointment, applicationUser }
                )
                .ToListAsync();


            return Ok(new
            {
                appointments,
                message = "Doctors Appointments"
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
                    appointment => appointment.PatientId,
                    applicationUser => applicationUser.Id,
                    (appointment, applicationUser) => new { appointment, applicationUser }
                )
                .Join(
                    _applicationDbContext.PatientProfiles,
                    applicationUser => applicationUser.applicationUser.Id,
                    patientProfile => patientProfile.PatientId,
                    (applicationUser, patientProfile) => new { applicationUser, patientProfile }
                )
                .ToListAsync();


            return Ok(new
            {
                appointment,
                message = "Complete Appointment Details Appointments"
            });

        }

        [Route("AcceptAnAppointment")]
        [HttpPatch]
        public async Task<IActionResult> AcceptAnAppointment(string AppointmentId)
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
                appointment.IsAccepted = true;
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Appointment Accepted Successfully"
                });
            }



        }

        [Route("RejectAnAppointment")]
        [HttpPatch]
        public async Task<IActionResult> RejectAnAppointment(string AppointmentId)
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
                appointment.IsRejected = true;
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Appointment Rejected Successfully"
                });
            }



        }

        //only happens when an appointment have ealier been accepted
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

                if (appointment.IsAccepted == false)
                {
                    return BadRequest(new
                    {
                        response = 401,
                        message = "You have to accept an appointment before you can cancel it.. Use reject appointment instead"
                    });
                }

                appointment.IsCanceledByDoctor = true;
                appointment.IsAccepted = false;
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Appointment Rejected Successfully"
                });
            }



        }

        [Route("ViewPatientMedicalHistory")]
        [HttpGet]
        public async Task<IActionResult> ViewPatientMedicalHistory(string PatientId)
        {
            //get all the prescriptions made for this patient
            var medicalHistory = await _applicationDbContext.DoctorAppointments
                .Where(s => s.PatientId == PatientId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    appointment => appointment.DoctorId,
                    doctor => doctor.Id,
                    (appointment, doctor) => new { appointment, doctor }
                )
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    appointment => appointment.appointment.PatientId,
                    patient => patient.Id,
                    (appointment, patient) => new { appointment, patient }
                )

                .ToListAsync();

            return Ok(new
            {
                medicalHistory,
                message = "Patient Medical History"
            });

        }


    }
}