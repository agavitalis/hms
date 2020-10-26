using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HMS.Areas.Patient.ViewModels.PatientProfileViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient")]
    [ApiController]
    public class PatientProfileController : Controller
    {
        private readonly IPatientProfile _patientRepository;

        public PatientProfileController(IPatientProfile patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [Route("GetPatient")]
        [HttpGet]
        public async Task<IActionResult> GetPatientAsync(string id)
        {

            var patientProfile = await _patientRepository.GetPatientByIdAsync(id);

            if (patientProfile != null)
            {
                return Ok(new
                {
                    patientProfile

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id"
                });
            }
        }

        [Route("GetPatientProfile")]
        [HttpGet]
        public async Task<IActionResult> GetPatientProfileAsync(string id)
        {

            var patientProfile = await _patientRepository.GetPatientProfileByIdAsync(id);

            return Ok(new
            {
                patientProfile

            });

        }

       
        [HttpPost]
        [Route("UpdatePatientContactDetails")]
        public async Task<IActionResult> EditPatientAddressAsync([FromBody] PatientAddressViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientAddressAsync(patient))
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

       
        [Route("UpdatePatientProfilePicture")]
        [HttpPost()]
        public async Task<IActionResult> EditPatientProfilePictureAsync([FromBody] PatientProfilePictureViewModel patientProfile, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientProfilePictureAsync(patientProfile))
                {
                    return Ok(new
                    {
                        message = "Profile Updated Successfully"
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
