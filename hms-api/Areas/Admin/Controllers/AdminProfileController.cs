using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Profile")]
    [ApiController]
    public class AdminProfileController : Controller
    {
        private readonly IAdminProfile _adminProfile;

        public AdminProfileController(IAdminProfile adminProfile)
        {
            _adminProfile = adminProfile;
        }



        [Route("GetAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAdmin(string AdminId)
        {

            var admin = await _adminProfile.GetAdmin(AdminId);
            if (admin != null)
            {
                return Ok(new
                {
                    admin
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Admin Id"
                });
            }

        }

        [Route("GetAdmins")]
        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _adminProfile.GetAdmins();

            return Ok(new
            {
                admins
            });

        }

        [HttpPost]
        [Route("UpdateAdminBasicInfo")]
        public async Task<IActionResult> EditAdminAsync([FromBody] EditAdminBasicInfoViewModel admin)
        {
            if (ModelState.IsValid)
            {
                if (await _adminProfile.EditAdminBasicInfo(admin))
                {
                    return Ok(new
                    {
                        message = "Admin record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Admin details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateAdminContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] EditAdminContactDetailsViewModel admin)
        {
            if (ModelState.IsValid)
            {
                if (await _adminProfile.EditAdminContactDetails(admin))
                {
                    return Ok(new
                    {
                        message = "Admin record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Admin details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("EditAccountProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditAdminProfilePictureAsync([FromBody] AdminProfilePictureViewModel AdminProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _adminProfile.EditAdminProfilePictureAsync(AdminProfile))
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
