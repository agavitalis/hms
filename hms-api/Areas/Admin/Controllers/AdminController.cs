using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Models.Doctor;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adminRepo;
        private readonly IUser _userRepo;
        private readonly IMapper _mapper;

        public AdminController(IAdmin adminRepo, IUser userRepo, IMapper mapper)
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

        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _adminRepo.GetAllDoctors();

            return Ok(new
            {
                doctors,
                message = "Complete Doctors List"
            });
        }

        [Route("ViewADoctorProfile")]
        [HttpGet]
        public async Task<IActionResult> ViewADoctorProfile(string doctorId)
        {
            var doctorProfile = await _adminRepo.GetDoctorsById(doctorId);

            if (doctorProfile != null)
            {
                return Ok(new
                {
                    doctorProfile,
                    message = "Success"
                });
            }
            else
            {
                return NotFound(new
                {
                    response = 401,
                    message = "Invalid Credentials"
                });
            }


        }

    }
}
