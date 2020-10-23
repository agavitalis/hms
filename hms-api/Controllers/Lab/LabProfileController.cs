using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Services.Interfaces.Lab;
using HMS.ViewModels.Lab;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabProfileController : Controller
    {
        private readonly ILabProfile _labProfile;

        public LabProfileController(ILabProfile labProfile)
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
