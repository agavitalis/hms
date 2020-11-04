using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient", Name = "Patient- Manage Doctor Consultation")]
    [ApiController]
    public class PatientConsultationController : Controller
    {
        private readonly IPatientConsultation _patientConsultation;

        public PatientConsultationController(IPatientConsultation patientConsultation)
        {
            _patientConsultation = patientConsultation;
        }

        [Route("BookConsultationWithADoctor")]
        [HttpPost]
        public async Task<IActionResult> BookConsultationWithADoctor([FromBody] AddPatientToADoctorConsultationListViewModel PatientQueue)
        {
            if (await _patientConsultation.AddPatientToADoctorConsultationList(PatientQueue))
            {
                return Ok(new
                {
                    message = "Patient Successfully Added To Queue"

                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid PatientId"
                });
            }
        }

        [Route("GetAPatientConsultationList")]
        [HttpGet]
        public async Task<IActionResult> GetAPatientConsultationList(string patientId)
        {
            var patientConsultationList = await _patientConsultation.GetAPatientConsultationList(patientId);

            if (patientConsultationList != null)
            {
                return Ok(new
                {
                    patientConsultationList,
                    message = "Patient Queue For Today"
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

        [Route("CancelConsultation")]
        [HttpPatch]
        public async Task<IActionResult> CancelConsultation(string patientQueueId)
        {
            var response = await _patientConsultation.CancelPatientConsultationAsync(patientQueueId);
            
            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Consultation succesfully Cancelled"

                });
            }
            else if (response == 1)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid PatientQueueId"
                });
            }
            else if (response == 2)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Expired"
                });
            }
            else if (response == 3)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Completed"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }
        }

        [Route("ExpireConsultation")]
        [HttpPatch]
        public async Task<IActionResult> ExpireConsultation(string patientQueueId)
        {
            var response = await _patientConsultation.ExpirePatientConsultationAsync(patientQueueId);
            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Consultation succesfully Expired"

                });
            }
            else if (response == 1)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid PatientQueueId"
                });
            }
            else if (response == 2)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Completed"
                });
            }
            else if (response == 3)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Canceled"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }
        }

        [Route("CompleteConsultation")]
        [HttpPatch]
        public async Task<IActionResult> CompleteConsultation(string patientQueueId)
        {
            var response = await _patientConsultation.CompletePatientConsultationAsync(patientQueueId);
            if (response == 0)
            {
                return Ok(new
                {
                    message = "Patient Consultation succesfully Completed"

                });
            }
            else if (response == 1)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid PatientQueueId"
                });
            }
            else if (response == 2)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Expired"
                });
            }
            else if (response == 3)
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Consultation Is Already Canceled"
                });
            }
           
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "There was an error contact the administrator"
                });
            }
        }
    
    }
}
