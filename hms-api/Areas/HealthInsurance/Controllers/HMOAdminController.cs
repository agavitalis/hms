using System.Threading.Tasks;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage Profile")]
    [ApiController]
    public class HMOAdminController : Controller
    {
        private readonly IHMOAdmin _HMOAdmin;

        public HMOAdminController(IHMOAdmin HMOAdmin)
        {
            _HMOAdmin = HMOAdmin;
        }

        [HttpGet("GetHMOAdmins")]
        public async Task<IActionResult> GetHMOAdmins([FromQuery] PaginationParameter paginationParameter)
        {
            var HMOAdmins = _HMOAdmin.GetHMOAdmins(paginationParameter);

            var paginationDetails = new
            {
                HMOAdmins.TotalCount,
                HMOAdmins.PageSize,
                HMOAdmins.CurrentPage,
                HMOAdmins.TotalPages,
                HMOAdmins.HasNext,
                HMOAdmins.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOAdmins,
                paginationDetails,
                message = "HMO Admins Returned"
            });

        }

        [Route("GetHMOAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetNurse(string HMOAdminId)
        {

            var HMOAdmin = await _HMOAdmin.GetHMOAdmin(HMOAdminId);
            if (HMOAdmin != null)
            {
                return Ok(new
                {
                    HMOAdmin
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid HMO Admin Id"
                });
            }

        }


        [Route("UpdateBasicInfo")]
        [HttpPost]
        public async Task<IActionResult> EditNurseAsync([FromBody] HMOAdminBasicInfoDtoForUpdate HMOAdmin)
        {
            if (ModelState.IsValid)
            {
                if (await _HMOAdmin.UpdateBasicInfo(HMOAdmin))
                {
                    return Ok(new
                    {
                        message = "HMO Admin Record Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update HMO Admin details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateContactDetails")]
        public async Task<IActionResult> EditNurseAddressAsync([FromBody] HMOAdminContactDetailsDtoForUpdate HMOAdmin)
        {
            if (ModelState.IsValid)
            {
                if (await _HMOAdmin.UpdateContactDetails(HMOAdmin))
                {
                    return Ok(new
                    {
                        message = "HMO Admin Record Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update HMO Admin details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("EditProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> UpdateNurseProfilePictureAsync([FromForm] HMOAdminProfilePictureDtoForUpdate HMOAdmin)
        {
            if (ModelState.IsValid)
            {
                if (await _HMOAdmin.UpdateProfilePictureAsync(HMOAdmin))
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
