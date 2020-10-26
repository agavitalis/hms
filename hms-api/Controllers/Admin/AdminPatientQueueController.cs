using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.Areas.Patient.ViewModels.PatientQueueViewModel;

namespace HMS.Controllers.Admin
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminPatientQueueController : Controller
    {
        private readonly IPatientQueue _patientQueue;
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminPatientQueueController(IPatientQueue patientQueue, ApplicationDbContext applicationDbContext)
        {
            _patientQueue = patientQueue;
            _applicationDbContext = applicationDbContext;
        }


        [Route("AddPatientToQueue")]
        [HttpPost]
        public async Task<IActionResult> AddPatientToQueue([FromBody] AdminAddPatientToQueueViewModel patientQueue)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Email == patientQueue.PatientEmail);

            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //add patient to queue
                var queue = new PatientQueue()
                {
                    ConsultationTitle = patientQueue.ConsultationTitle,
                    ReasonForConsultation = patientQueue.ReasonForConsultation,
                    PatientId = Patient.Id,
                    DoctorId = patientQueue.DoctorId
                };


                _applicationDbContext.PatientQueue.Add(queue);
                await _applicationDbContext.SaveChangesAsync();

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
                    message = "Invalid Patient Email Supplied"
                });
            }
        }

        [Route("GetPatientQueue")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var PatientQueue = await  _applicationDbContext.PatientQueue.Where(p => p.DateOfConsultation.Date == DateTime.Today)
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
