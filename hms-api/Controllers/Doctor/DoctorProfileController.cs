using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models;
using HMS.Services.Interfaces.Doctor;
using HMS.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Doctor.DoctorSpecializationViewModel;

namespace HMS.Controllers.Doctor
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorProfileController : Controller
    {
        private readonly IDoctorProfile _doctorProfile;

        public DoctorProfileController(IDoctorProfile doctorProfile)
        {
            _doctorProfile = doctorProfile;
        }



        [Route("GetDoctor")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorByIdAsync(string DoctorId)
        {

            var doctor = await _doctorProfile.GetDoctorByIdAsync(DoctorId);
            if (doctor != null)
            {
                return Ok(new
                {
                    doctor
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Doctor Id"
                });
            }

        }

        [Route("GetDoctorProfile")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorProfileByIdAsync(string DoctorId)
        {

            var doctorProfile = await _doctorProfile.GetDoctorProfileByIdAsync(DoctorId);

            return Ok(new
            {
                doctorProfile
            });

        }

        [Route("GetDoctorSpecialization")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorSpecializationByIdAsync(string DoctorId)
        {

            var doctorSpecialization = await _doctorProfile.GetDoctorSpecializationByIdAsync(DoctorId);

            return Ok(new
            {
                doctorSpecialization
            });


        }


        [Route("EditDoctorProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorProfilePictureAsync([FromBody] DoctorProfilePictureViewModel DoctorProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorProfilePictureAsync(DoctorProfile))
                {
                    return Ok(new
                    {
                        message = "Profile picture Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 400,
                        message = "Failed to Update profile picture"
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Parameters"
                });
            }
        }


        [HttpPost]
        [Route("UpdateDoctorBasicInfo")]
        public async Task<IActionResult> EditDoctorBasicInfoAsync([FromBody] EditDoctorBasicInfoViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorBasicInfoAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Doctor Basic Info Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateDoctorContactDetails")]
        public async Task<IActionResult> EditDoctorContactAsync([FromBody] DoctorContactViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorContactAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Doctor Address Successfully Saved"
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
        [Route("UpdateDoctorProfessionalDetails")]
        public async Task<IActionResult> EditDoctorProfessionalDetails([FromBody] DoctorProfessionalDetailsViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorProfessionalDetailsAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Doctor Professional Details Successfully Saved"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert  details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete Details" });
        }

        [HttpPost]
        [Route("UpdateDoctorAvaliabilityDetails")]
        public async Task<IActionResult> EditDoctorAvaliablityDetails([FromBody] DoctorAvaliablityViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorAvaliabilityAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Avaliability Details Successfully Updated"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to update  details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete Details" });
        }



    }
}