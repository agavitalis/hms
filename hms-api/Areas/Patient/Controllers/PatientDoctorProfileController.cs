using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient")]
    [ApiController]
    public class PatientDoctorProfileController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PatientDoctorProfileController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
          
        }

        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            var doctors = await  _applicationDbContext.ApplicationUsers.Where(a => a.UserType == "Doctor")
                     .Join(
                          _applicationDbContext.DoctorProfiles,
                          applicationUser => applicationUser.Id,
                          DoctorProfile => DoctorProfile.DoctorId,
                          (applicationUser, DoctorProfile) => new { applicationUser, DoctorProfile }
                      )
                     .ToListAsync();


            if (doctors != null)
            {
                return Ok(new
                {
                    doctors

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 404,
                    message = "No doctors found"
                });
            }

        }

        [Route("ViewADoctorProfile")]
        [HttpGet]
        public async Task<IActionResult> ViewADoctorProfileAsync(string DoctorId)
        {
            var DoctorProfile =await  _applicationDbContext.ApplicationUsers.Where(p => p.Id == DoctorId)
                      .Join(
                          _applicationDbContext.DoctorProfiles,
                          applicationUser => applicationUser.Id,
                          DoctorProfile => DoctorProfile.DoctorId,
                          (applicationUser, DoctorProfile) => new { applicationUser, DoctorProfile }
                      ).FirstAsync();

            if (DoctorProfile != null)
            {
                return Ok(new
                {
                    DoctorProfile

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 401,
                    message = "Invalid Credentials"
                });
            }
        }
        
    }
}
