using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HMS.Areas.Patient.ViewModels.PatientProfileViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient", Name = "Patient- Manage Profile")]
    [ApiController]
    public class PatientProfileController : Controller
    {
        private readonly IPatientProfile _patientRepository;

        public PatientProfileController(IPatientProfile patientRepository)
        {
            _patientRepository = patientRepository;
        }


        [Route("GetPatients", Name = "Patients")]
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var Patients = await _patientRepository.GetPatientsAsync();

            if (Patients != null)
            {
                return Ok(new
                {
                    Patients,
                    message = "Complete Patient List"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Credentials Passed"
                });
            }
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

        [HttpPost]
        [Route("UpdatePatientBasicInfo")]
        public async Task<IActionResult> EditPatientAsync([FromBody] EditPatientBasicInfoViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientBasicInfoAsync(patient))
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

        [HttpPost]
        [Route("UpdatePatientHealthDetails")]
        public async Task<IActionResult> EditPatientHealthAsync([FromBody] PatientHealthViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (await _patientRepository.EditPatientHealthAsync(patient))
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
