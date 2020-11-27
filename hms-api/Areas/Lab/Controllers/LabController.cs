using System;
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

        
        [Route("GetALabTechnician")]
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
            var labTechnicians = await _labProfile.GetLabProfiles();

            return Ok(new
            {
                labTechnicians
            });

        }

        [HttpPost]
        [Route("UpdateLabProfileBasicInfo")]
        public async Task<IActionResult> EditPatientAsync([FromBody] EditLabProfileBasicInfoViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _labProfile.EditLabProfileBasicInfoAsync(patient))
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
        [Route("UpdateLabProfileContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] EditLabProfileContactDetailsViewModel labProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _labProfile.EditLabProfileContactDetailsAsync(labProfile))
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
