using System.Threading.Tasks;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Profile")]
    [ApiController]
    public class WardPersonnelProfileController : Controller
    {
        private readonly IWardPersonnel _wardPersonnel;

        public WardPersonnelProfileController(IWardPersonnel wardPersonnel)
        {
            _wardPersonnel = wardPersonnel;
        }

        [HttpGet("GetWardPersonnels")]
        public async Task<IActionResult> GetWardPersonnels([FromQuery] PaginationParameter paginationParameter)
        {
            var wardPersonnels = _wardPersonnel.GetWardPersonnels(paginationParameter);

            var paginationDetails = new
            {
                wardPersonnels.TotalCount,
                wardPersonnels.PageSize,
                wardPersonnels.CurrentPage,
                wardPersonnels.TotalPages,
                wardPersonnels.HasNext,
                wardPersonnels.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                wardPersonnels,
                paginationDetails,
                message = "Ward Personnels Returned"
            });

        }

        [Route("GetWardPersonnel")]
        [HttpGet]
        public async Task<IActionResult> GetNurse(string WardPersonnelId)
        {

            var wardPersonnel = await _wardPersonnel.GetWardPersonnel(WardPersonnelId);
            if (wardPersonnel != null)
            {
                return Ok(new
                {
                    wardPersonnel
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Ward Personnel Id"
                });
            }

        }


        [Route("UpdateBasicInfo")]
        [HttpPost]
        public async Task<IActionResult> EditNurseAsync([FromBody] WardPersonnelBasicInfoDtoForUpdate WardPersonnel)
        {
            if (ModelState.IsValid)
            {
                if (await _wardPersonnel.UpdateBasicInfo(WardPersonnel))
                {
                    return Ok(new
                    {
                        message = "Ward Personnel Record Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update Ward Personnel details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateContactDetails")]
        public async Task<IActionResult> EditNurseAddressAsync([FromBody] WardPersonnelContactDetailsDtoForUpdate WardPersonnel)
        {
            if (ModelState.IsValid)
            {
                if (await _wardPersonnel.UpdateContactDetails(WardPersonnel))
                {
                    return Ok(new
                    {
                        message = "Ward Personnel Record Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update Ward Personnel details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("EditProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> UpdateNurseProfilePictureAsync([FromForm] WardPersonnelProfilePictureDtoForUpdate WardPersonnel)
        {
            if (ModelState.IsValid)
            {
                if (await _wardPersonnel.UpdateProfilePictureAsync(WardPersonnel))
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
