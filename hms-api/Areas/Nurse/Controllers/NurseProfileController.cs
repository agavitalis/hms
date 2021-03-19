using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Interfaces.Nurse;
using HMS.Areas.Nurse.Dtos;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Nurse.Controllers
{
    [Route("api/Nurse", Name = "Nurse - Manage Profile")]
    [ApiController]
    public class NurseProfileController : Controller
    {
        private readonly INurse _nurse;

        public NurseProfileController(INurse nurse)
        {
            _nurse = nurse;
        }

        [HttpGet("GetNurses")]
        public async Task<IActionResult> GetNurses([FromQuery] PaginationParameter paginationParameter)
        {
            var nurses = _nurse.GetNurses(paginationParameter);

            var paginationDetails = new
            {
                nurses.TotalCount,
                nurses.PageSize,
                nurses.CurrentPage,
                nurses.TotalPages,
                nurses.HasNext,
                nurses.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                nurses,
                paginationDetails,
                message = "Nurses Returned"
            });

        }

        [Route("GetNurse")]
        [HttpGet]
        public async Task<IActionResult> GetNurse(string NurseId)
        {

            var nurse = await _nurse.GetNurse(NurseId);
            if (nurse != null)
            {
                return Ok(new
                {
                    nurse
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Nurse Id"
                });
            }

        }


        [Route("UpdateBasicInfo")]
        [HttpPost]
        public async Task<IActionResult> EditNurseAsync([FromBody] NurseBasicInfoDtoForEdit nurse)
        {
            if (ModelState.IsValid)
            {
                if (await _nurse.EditBasicInfo(nurse))
                {
                    return Ok(new
                    {
                        message = "Nurse record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Nurse details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateContactDetails")]
        public async Task<IActionResult> EditNurseAddressAsync([FromBody] NurseContactDetailsDtoForEdit nurse)
        {
            if (ModelState.IsValid)
            {
                if (await _nurse.EditContactDetails(nurse))
                {
                    return Ok(new
                    {
                        message = "Nurse record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Nurse details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("EditProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditNurseProfilePictureAsync([FromForm] NurseProfilePictureDtoForEdit nurse)
        {
            if (ModelState.IsValid)
            {
                if (await _nurse.EditProfilePictureAsync(nurse))
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

