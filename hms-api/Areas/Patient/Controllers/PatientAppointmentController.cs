using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient", Name = "Patient- Manage Doctor Appointments")]
    [ApiController]
    public class PatientAppointmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUser _userRepo;
        private readonly IPatientAppointment _appointment;
        public PatientAppointmentController(ApplicationDbContext applicationDbContext, IUser userRepo, IPatientAppointment appointment)
        {
            _applicationDbContext = applicationDbContext;
            _appointment = appointment;
            _userRepo = userRepo;

        }

        [Route("GetPendingAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPendingAppoinmentsCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetPendingAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCompletedAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsUnattendedToCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetCompletedAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCanceledAppointmentsCount")]
        [HttpGet]
        public async Task<IActionResult> GetCanceledAppointmentsCount(string patientId)
        {
            var appointmentsCount = await _appointment.GetCanceledAppointmentsCount(patientId);

            return Ok(new
            {
                appointmentsCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("BookAppointment")]
        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel appointment)
        {
            //check if this guy has a profile already
            var patient = await _userRepo.GetUserByIdAsync(appointment.PatientId);
            var doctor = await _userRepo.GetUserByIdAsync(appointment.DoctorId);
            if (patient != null && doctor != null)
            {
                //if its avaliable now book it
                var doctorAppointment = new Appointment()
                {

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
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id or Doctor Id Supplied"
                });
            }
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