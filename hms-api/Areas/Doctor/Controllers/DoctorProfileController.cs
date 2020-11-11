﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models;
using HMS.Areas.Doctor.Controllers;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static HMS.Areas.Doctor.ViewModels.DoctorProfilePictureViewModel;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor- Manage Profile")]
    [ApiController]
    public class DoctorProfileController : Controller
    {
        private readonly IDoctorProfile _doctorProfile;

        public DoctorProfileController(IDoctorProfile doctorProfile)
        {
            _doctorProfile = doctorProfile;
        }



        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {

            var doctors = await _doctorProfile.GetDoctorsAsync();
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
                    response = 400,
                    message = "Invalid Doctor Id"
                });
            }

        }

        [Route("GetDoctor")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorByIdAsync(string DoctorId)
        {

            var doctorProfile = await _doctorProfile.GetDoctorAsync(DoctorId);

            return Ok(new
            {
                doctorProfile
            });

        }

        [Route("EditDoctorProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorProfilePictureAsync([FromForm] DoctorProfilePictureViewModel DoctorProfile)
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