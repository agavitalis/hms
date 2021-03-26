using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/Lab", Name = "Lab - Manage Profile")]
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

            var labTechnician = await _labProfile.GetLabAttendant(id);
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
        public async Task<IActionResult> GetAllLabTechniciansAsync([FromQuery] PaginationParameter paginationParameter)
        {
            var labAttendant = _labProfile.GetLabAttendants(paginationParameter);

            var paginationDetails = new
            {
                labAttendant.TotalCount,
                labAttendant.PageSize,
                labAttendant.CurrentPage,
                labAttendant.TotalPages,
                labAttendant.HasNext,
                labAttendant.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                labAttendant,
                paginationDetails,
                message = "Nurses Returned"
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
