using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor _doctorRepo;
       

        public DoctorController(IDoctor doctorRepo)
        {
            _doctorRepo = doctorRepo;
          
        }

        
        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _doctorRepo.GetAllDoctors();

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
            var doctorProfile = await _doctorRepo.GetDoctorsById(doctorId);

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
