using HMS.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Controllers.Doctor
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorPatientProfileController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorPatientProfileController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Route("GetAllPatients")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var PatientProfiles = await _applicationDbContext.ApplicationUsers.Where(p => p.UserType == "Patient")
                                  .Join(
                                      _applicationDbContext.PatientProfiles,
                                      applicationUser => applicationUser.Id,
                                      PatientProfile => PatientProfile.PatientId,
                                      (applicationUser, PatientProfile) => new { applicationUser, PatientProfile }
                                  )
                                  .FirstAsync();

            if (PatientProfiles != null)
            {
                return Ok(new
                {
                    PatientProfiles,
                    message = "Complete Patient List"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Credentials Passed"
                });
            }
        }

        [Route("GetPatientById")]
        [HttpGet]
        public async Task<IActionResult> GetPatientByIdAsync(string PatientId)
        {
            var PatientProfile = await _applicationDbContext.ApplicationUsers.Where(p => p.Id == PatientId)
                                  .Join(
                                      _applicationDbContext.PatientProfiles,
                                      applicationUser => applicationUser.Id,
                                      PatientProfile => PatientProfile.PatientId,
                                      (applicationUser, PatientProfile) => new { applicationUser, PatientProfile }
                                  )
                                  .FirstAsync();

            if (PatientProfile != null)
            {
                return Ok(new
                {
                    PatientProfile,
                    message = "Complete Patient Profile"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Credentials Passed"
                });
            }
        }

    }
}