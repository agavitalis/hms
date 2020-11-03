using System.Threading.Tasks;
using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Accountant.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant", Name = "Accountant- Manage Profile")]
    [ApiController]
    public class AccountantProfileController : Controller
    {
        private readonly IAccountantProfile _accountProfile;

        public AccountantProfileController(IAccountantProfile accountProfile)
        {
            _accountProfile = accountProfile;
        }

        

        [Route("GetAnAccountProfile")]
        [HttpGet]
        public async Task<IActionResult> GetAccountProfileByIdAsync(string AccountantId)
        {
           
            var Accountant = await _accountProfile.GetAccountantByIdAsync(AccountantId);
            if (Accountant != null)
            {
                return Ok(new
                {
                    Accountant
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Account Profile Id Or Profile Has Not Been Filled"
                });
            }
            
        }

        [Route("EditAccountantProfile")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorAsync([FromBody] EditAccountProfileViewModel AccountProfile)
        {
           
            if (await _accountProfile.EditAccountProfileAsync(AccountProfile))
            {
                return Ok(new
                {
                    message = "Account Profile Updated Successfully"
                });

            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to Update Account Profile"
                });
            }
           
        }

        [Route("EditAccountProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorProfilePictureAsync([FromBody] AccountProfilePictureViewModel AccountProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _accountProfile.EditAccountProfilePictureAsync(AccountProfile))
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
