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



        [Route("GetAccountant")]
        [HttpGet]
        public async Task<IActionResult> GetAccountant(string AccountantId)
        {

            var accountant = await _accountProfile.GetAccountant(AccountantId);
            if (accountant != null)
            {
                return Ok(new
                {
                    accountant
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Accountant Id"
                });
            }

        }

        [Route("GetAccountants")]
        [HttpGet]
        public async Task<IActionResult> GetAccountants()
        {
            var labTechnicians = await _accountProfile.GetAccountants();

            return Ok(new
            {
                labTechnicians
            });

        }

        [HttpPost]
        [Route("UpdateAccountactBasicInfo")]
        public async Task<IActionResult> EditPatientAsync([FromBody] EditAccountantBasicInfoViewModel accountant)
        {
            if (ModelState.IsValid)
            {
                if (await _accountProfile.EditAccountantBasicInfo(accountant))
                {
                    return Ok(new
                    {
                        message = "Accountant record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Accountant details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateAccountantContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] EditAccountantContactDetailsViewModel accountant)
        {
            if (ModelState.IsValid)
            {
                if (await _accountProfile.EditAccountantContactDetails(accountant))
                {
                    return Ok(new
                    {
                        message = "Accountant record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Accountant details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("EditAccountProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditProfilePictureAsync([FromBody] AccountProfilePictureViewModel AccountProfile)
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
