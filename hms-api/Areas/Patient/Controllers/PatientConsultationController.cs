using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient", Name = "Patient- Manage Consultation")]
    [ApiController]
    public class PatientConsultationController : Controller
    {
        private readonly IPatientConsultation _consultation;
        private readonly IPatientProfile _patient;

        public PatientConsultationController(IPatientConsultation consultation, IPatientProfile patient)
        {
            _consultation = consultation;
            _patient = patient;
        }

        [Route("GetPendingConsultationsCount")]
        [HttpGet]
        public async Task<IActionResult> GetConsultationCount(string patientId)
        {
            var consultationCount = await _consultation.GetPendingConsultationsCount(patientId);

            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCompletedConsultationsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsUnattendedToCount(string patientId)
        {
            var consultationCount = await _consultation.GetCompletedConsultationsCount(patientId);

            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetCanceledConsultationsCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsAttendedToCount(string patientId)
        {
            var consultationCount = await _consultation.GetCanceledConsultationsCount(patientId);

            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("BookConsultation")]
        [HttpPost]
        public async Task<IActionResult> BookConsultation([FromBody] BookConsultation patientConsultation)
        {
            if (await _consultation.BookConsultation(patientConsultation))
            {
                var myPatient = new MyPatient();

                myPatient = new MyPatient()
                {
                    DoctorId = patientConsultation.DoctorId,
                    PatientId = patientConsultation.PatientId,
                    DateCreated = DateTime.Now
                };
                var result = await _consultation.AssignDoctorToPatient(myPatient);
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

        [Route("GetCompletedConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetCompletedConsultations([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }
            var consultations = _consultation.GetCompletedConsultations(PatientId, paginationParameter);

            var paginationDetails = new
            {
                consultations.TotalCount,
                consultations.PageSize,
                consultations.CurrentPage,
                consultations.TotalPages,
                consultations.HasNext,
                consultations.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                consultations,
                paginationDetails,
                message = "Appointments returned"
            });
        }

        [Route("GetCanceledConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetPendingConsultations([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }
            var consultations = _consultation.GetCanceledConsultations(PatientId, paginationParameter);

            var paginationDetails = new
            {
                consultations.TotalCount,
                consultations.PageSize,
                consultations.CurrentPage,
                consultations.TotalPages,
                consultations.HasNext,
                consultations.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                consultations,
                paginationDetails,
                message = "Consultations returned"
            });
        }


        [Route("GetPendingConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetCanceledConsultations([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patient.GetPatientByIdAsync(PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }

            var consultations = _consultation.GetPendingConsultations(PatientId, paginationParameter);

            var paginationDetails = new
            {
                consultations.TotalCount,
                consultations.PageSize,
                consultations.CurrentPage,
                consultations.TotalPages,
                consultations.HasNext,
                consultations.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                consultations,
                paginationDetails,
                message = "Consultations returned"
            });
        }

        

        [Route("CancelConsultation")]
        [HttpPatch]
        public async Task<IActionResult> CancelConsultation(string patientQueueId)
        {
            var response = await _consultation.CancelPatientConsultationAsync(patientQueueId);
            
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
            var response = await _consultation.ExpirePatientConsultationAsync(patientQueueId);
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
            var response = await _consultation.CompletePatientConsultationAsync(patientQueueId);
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
