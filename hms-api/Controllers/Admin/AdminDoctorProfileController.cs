using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Doctor;
using HMS.Services.Interfaces.Patient;
using HMS.ViewModels;
using HMS.ViewModels.Doctor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.ViewModels.Patient.PatientProfileViewModel;

namespace HMS.Controllers.Admin
{
    [Route("api/Admin")]
    [ApiController]

    public class AdminDoctorProfileController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminDoctorProfileController( ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _applicationDbContext.ApplicationUsers.Where(d => d.UserType == "Doctor").ToListAsync();

           
            return Ok(new
            {
                doctors,
                message = "Complete Doctors List"
            });
            
           
        }

        [Route("ViewADoctorProfile")]
        [HttpGet]
        public async Task<IActionResult> ViewADoctorProfile(string DoctorId)
        {

            var DoctorProfile = await _applicationDbContext.ApplicationUsers.Where(p => p.Id == DoctorId)
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
