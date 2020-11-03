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

        [Route("EditPharmacyProfile")]
        [HttpPost]
        public async Task<IActionResult> EditCashierAsync([FromBody] EditPharmacyProfileViewModel PharmacyProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _pharmacyProfile.EditPharmacyProfileAsync(PharmacyProfile))
                {
                    return Ok(new
                    {
                        message = "Pharmasist Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Update Pharmasist"
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