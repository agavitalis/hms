using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin- Manage Consultations")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IPatientConsultation _patientConsultation;
        private readonly ApplicationDbContext _applicationDbContext;
    

        public ConsultationController(IPatientConsultation patientConsultation, ApplicationDbContext applicationDbContext)
        {
            _patientConsultation = patientConsultation;
            _applicationDbContext = applicationDbContext;
            
        }

        [Route("BookConsultation")]
        [HttpPost]
        public async Task<IActionResult> BookConsultation([FromBody] BookConsultation patientConsultation)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == patientConsultation.PatientId);

            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //add patient to queue
                var consultation = new Consultation()
                {
                    ConsultationTitle = patientConsultation.ConsultationTitle,
                    ReasonForConsultation = patientConsultation.ReasonForConsultation,
                    PatientId = patientConsultation.PatientId,
                    DoctorId = patientConsultation.DoctorId
                };


                _applicationDbContext.Consultations.Add(consultation);
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Patient Consultation Booked"

                });
            }

            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Email Supplied"
                });
            }
        }

        [Route("GetPatientConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var patientConsultations = await _applicationDbContext.Consultations
                 .Join(
                           _applicationDbContext.ApplicationUsers,
                           PatientQueue => PatientQueue.PatientId,
                           applicationUsers => applicationUsers.Id,
                           (PatientQueue, patient) => new { PatientQueue, patient }
                       )

                        .Join(
                            _applicationDbContext.ApplicationUsers,
                            PatientQueue => PatientQueue.PatientQueue.DoctorId,
                           applicationUsers => applicationUsers.Id,
                            (PatientQueue, doctor) => new { PatientQueue.PatientQueue, PatientQueue.patient, doctor }
                       )

                        .ToListAsync();


            if (patientConsultations != null)
            {
                return Ok(new
                {
                    patientConsultations,
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
        public async Task<IActionResult> CancelConsultation(string consultationId)
        {
            var response = await _patientConsultation.CancelPatientConsultationAsync(consultationId);

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
        public async Task<IActionResult> ExpireConsultation(string consultationId)
        {
            var response = await _patientConsultation.ExpirePatientConsultationAsync(consultationId);
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
        public async Task<IActionResult> CompleteConsultation(string consultationId)
        {
            var response = await _patientConsultation.CompletePatientConsultationAsync(consultationId);
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
