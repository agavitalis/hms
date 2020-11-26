using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy- Manage Profile")]
    [ApiController]
    public class PharmacyProfileController : Controller
    {
        private readonly IPharmacyProfile _pharmacyProfile;

        public PharmacyProfileController(IPharmacyProfile pharmacyProfile)
        {
            _pharmacyProfile = pharmacyProfile;
        }

        [Route("GetAPharmacistById")]
        [HttpGet]
        public async Task<IActionResult> GetPharmacyByIdAsync(string id)
        {
           
            var pharmacist = await _pharmacyProfile.GetPharmacyProfileByIdAsync(id);
            if (pharmacist != null)
            {
                return Ok(new
                {
                    pharmacist
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Pharmacy Id"
                });
            }
          
        }

        [Route("GetAllPharmacists")]
        [HttpGet]
        public async Task<IActionResult> GetAllPharmacyAsync()
        {
            var pharmacists = await  _pharmacyProfile.GetAllPharmacyAsync();
          
            return Ok(new
            {
                pharmacists
            });

        }

        [HttpPost]
        [Route("UpdatePharmacistBasicInfo")]
        public async Task<IActionResult> EditPharmacistBasicInfo([FromBody] EditPharmacistBasicInfoViewModel pharmacist)
        {
            if (ModelState.IsValid)
            {
                if (await _pharmacyProfile.EditPharmacistBasicInfoAsync(pharmacist))
                {
                    return Ok(new
                    {
                        message = "Pharmacist record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Pharmacist details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdatePharmacistContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] EditPharmacistContactDetailsViewModel pharmacist)
        {
            if (ModelState.IsValid)
            {
                if (await _pharmacyProfile.EditPharmacistContactDetailsAsync(pharmacist))
                {
                    return Ok(new
                    {
                        message = "Pharmacist record inserted Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert Pharmacist details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }


        [Route("EditPharmacyProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditPharmacyProfilePictureAsync([FromBody] PharmacyProfilePictureViewModel pharmacyProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _pharmacyProfile.EditPharmacyProfilePictureAsync(pharmacyProfile))
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