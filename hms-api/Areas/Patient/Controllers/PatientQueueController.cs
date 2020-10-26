using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientQueueController : Controller
    {
        private readonly IPatientQueue _patientQueue;

        public PatientQueueController(IPatientQueue patientQueue)
        {
            _patientQueue = patientQueue;
        }


        [Route("AddPatientToQueue")]
        [HttpPost]
        public async Task<IActionResult> AddPatientToQueue([FromBody] AddPatientToQueueViewModel PatientQueue)
        {
            if (await _patientQueue.AddPatientToQueueAsync(PatientQueue))
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

        [Route("GetPatientQueue")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueue()
        {
            var PatientQueue = await _patientQueue.GetPatientQueue();

            if (PatientQueue != null)
            {
                return Ok(new
                {
                    PatientQueue,
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
            var response = await _patientQueue.CancelPatientConsultationAsync(patientQueueId);
            
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
            var response = await _patientQueue.ExpirePatientConsultationAsync(patientQueueId);
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
            var response = await _patientQueue.CompletePatientConsultationAsync(patientQueueId);
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
