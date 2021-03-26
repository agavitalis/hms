using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/Patient", Name = "Patient - Manage Profile")]
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
        public async Task<IActionResult> GetPatients([FromQuery] PaginationParameter paginationParameter)
        {
            var patients = _patientRepository.GetPatients(paginationParameter);

            var paginationDetails = new
            {
                patients.TotalCount,
                patients.PageSize,
                patients.CurrentPage,
                patients.TotalPages,
                patients.HasNext,
                patients.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                patients,
                paginationDetails,
                message = "Patients Fetched"
            });
        }

        [Route("GetPatientsByDoctor")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsByDoctorAsync(string DoctorId)
        {

            var patients = await _patientRepository.GetPatientsByDoctorAsync(DoctorId);

            if (patients != null)
            {
                return Ok(new
                {
                    patients
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Doctor Id"
                });
            }

        }

        [Route("GetPatient")]
        [HttpGet]
        public async Task<IActionResult> GetPatientAsync(string id)
        {

            var patientProfile = await _patientRepository.GetPatient(id);

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

        [Route("GetPatientHealthHistory")]
        [HttpGet]
        public async Task<IActionResult> GetPatientHealthDetails(string PatientId)
        {

            var patientHealthHistory = await _patientRepository.GetPatientHealthHistory(PatientId);

            if (patientHealthHistory != null)
            {
                return Ok(new
                {
                    patientHealthHistory,
                    message= "Patient Health History"

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
        public async Task<IActionResult> EditPatientProfilePictureAsync([FromForm] PatientProfilePictureViewModel patientProfile )
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

        [HttpGet("SearchPatient/{searchParam}")]
        public IActionResult PatientSearch(string searchParam)
        {           
            var res = _patientRepository.SearchPatient(searchParam);
            if(res.Any())
                return Ok(new { res, message = "patients search completed" });

            return NotFound(new { res, message = "no patient found or patient does not exist" });
        }

    }
}
