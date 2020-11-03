﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/[controller]", Name = "Lab- Manage Profile")]
    [ApiController]
    public class LabController : ControllerBase
    {
        
        private readonly ILabProfile _labProfile;

        public LabController(ILabProfile labProfile)
        {
            _labProfile = labProfile;

        }

        
        [Route("GetALabTechnicianById")]
        [HttpGet]
        public async Task<IActionResult> GetLabByIdAsync(string id)
        {

            var labTechnician = await _labProfile.GetLabByIdAsync(id);
            if (labTechnician != null)
            {
                return Ok(new
                {
                    labTechnician
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Lab Technician Id"
                });
            }

        }

        [Route("GetAllLabTechnicians")]
        [HttpGet]
        public async Task<IActionResult> GetAllLabTechniciansAsync()
        {
            var labTechnicians = await _labProfile.GetAllLabAsync();

            return Ok(new
            {
                labTechnicians
            });

        }

        [Route("EditLabProfile")]
        [HttpPost]
        public async Task<IActionResult> EditLabTechnicianAsync([FromBody] EditLabProfileViewModel LabProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _labProfile.EditLabProfileAsync(LabProfile))
                {
                    return Ok(new
                    {
                        message = "Lab Profile Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update Lab Profile"
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


        [Route("EditLabProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditLabProfilePictureAsync([FromBody] LabProfilePictureViewModel labProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _labProfile.EditLabProfilePictureAsync(labProfile))
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
    }
}
