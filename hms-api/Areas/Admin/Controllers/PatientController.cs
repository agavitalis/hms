using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.Models;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientConsultation _patientQueue;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPatientProfile _patientRepository;

        public PatientController(IPatientConsultation patientQueue, ApplicationDbContext applicationDbContext, IPatientProfile patientRepository)
        {
            _patientQueue = patientQueue;
            _applicationDbContext = applicationDbContext;
            _patientRepository = patientRepository;
        }

        [Route("GetPatients", Name ="Patients")]
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var PatientProfiles = await _applicationDbContext.ApplicationUsers.Where(p => p.UserType == "Patient").ToListAsync();

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

        [Route("GetPatient")]
        [HttpGet]
        public async Task<IActionResult> GetPatientAsync(string id)
        {

            var patientProfile = await _patientRepository.GetPatientByIdAsync(id);

            if (patientProfile != null)
            {
                return Ok(new
                {
                    patientProfile

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id"
                });
            }

        }

        [Route("GetPatientProfile")]
        [HttpGet]
        public async Task<IActionResult> GetPatientProfileAsync(string id)
        {

            var patientProfile = await _patientRepository.GetPatientProfileByIdAsync(id);

            return Ok(new
            {
                patientProfile

            });
        }


        [HttpPost]
        [Route("UpdatePatientBasicInfo")]
        public async Task<IActionResult> EditPatientAsync([FromBody] EditPatientBasicInfoViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientBasicInfoAsync(patient))
                {
                    return Ok(new
                    {
                        message = "patient record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert patient details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdatePatientContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] PatientAddressViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientAddressAsync(patient))
                {
                    return Ok(new
                    {
                        message = "patient record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert patient details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdatePatientHealthDetails")]
        public async Task<IActionResult> EditPatientHealthAsync([FromBody] PatientHealthViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientHealthAsync(patient))
                {
                    return Ok(new
                    {
                        message = "patient record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert patient details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("UpdatePatientProfilePicture")]
        [HttpPost()]
        public async Task<IActionResult> EditPatientProfilePictureAsync([FromBody] PatientProfilePictureViewModel patientProfile, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientProfilePictureAsync(patientProfile))
                {
                    return Ok(new
                    {
                        message = "Profile Updated Successfully"
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
