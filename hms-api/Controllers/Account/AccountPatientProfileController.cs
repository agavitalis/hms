using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Services.Interfaces.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers.Account
{
    [Route("api/Accountant")]
    [ApiController]
    public class AccountPatientProfileController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountPatientProfileController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }


        [Route("GetAPatientProfile")]
        [HttpGet]
        public async Task<IActionResult> GetPatientProfileByIdAsync(string PatientId)
        {
            //check if this guy has a profile already
            var patientProfile = await _applicationDbContext.PatientProfiles.FirstOrDefaultAsync(a => a.PatientId == PatientId);
            if (patientProfile == null)
            {
                return null;
            }

            var patient = await _applicationDbContext.ApplicationUsers.Where(p => p.Id == PatientId)
                      .Join(
                          _applicationDbContext.PatientProfiles,
                          applicationUser => applicationUser.Id,
                          PatientProfile => PatientProfile.PatientId,
                          (applicationUser, PatientProfile) => new { applicationUser, PatientProfile }
                      )

                     .FirstAsync();
            if (patient != null)
            {
                return Ok(new
                {
                    patient

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 404,
                    message = "No patient found"
                });
            }          
        }

        [Route("GetAllPatientProfiles")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatientProfilesAsync()
        {
            var patients = await _applicationDbContext.ApplicationUsers
                      .Join(
                          _applicationDbContext.PatientProfiles,
                          applicationUser => applicationUser.Id,
                          PatientProfile => PatientProfile.PatientId,
                          (applicationUser, PatientProfile) => new { applicationUser, PatientProfile }
                      )
                      .ToListAsync();

            if (patients != null)
            {
                return Ok(new
                {
                    patients

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 404,
                    message = "No patient found"
                });
            }
        }
    }
}
