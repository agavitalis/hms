using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.Models;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
<<<<<<< HEAD
<<<<<<< HEAD:hms-api/Areas/Admin/Controllers/PatientController.cs
    public class PatientController : ControllerBase
=======
    public class ConsultationController : ControllerBase
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6:hms-api/Areas/Admin/Controllers/ConsultationController.cs
=======
    public class PatientController : ControllerBase
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
    {
        private readonly IPatientConsultation _patientQueue;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPatientProfile _patientRepository;

<<<<<<< HEAD
<<<<<<< HEAD:hms-api/Areas/Admin/Controllers/PatientController.cs
        public PatientController(IPatientQueue patientQueue, ApplicationDbContext applicationDbContext, IPatientProfile patientRepository)
=======
        public ConsultationController(IPatientConsultation patientQueue, ApplicationDbContext applicationDbContext, IPatientProfile patientRepository)
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6:hms-api/Areas/Admin/Controllers/ConsultationController.cs
=======
        public PatientController(IPatientConsultation patientQueue, ApplicationDbContext applicationDbContext, IPatientProfile patientRepository)
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
        {
            _patientQueue = patientQueue;
            _applicationDbContext = applicationDbContext;
            _patientRepository = patientRepository;
        }

<<<<<<< HEAD

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
                var queue = new PatientConsultation()
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

=======
        [Route("GetPatients", Name ="Patients")]
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var PatientProfiles = await _applicationDbContext.ApplicationUsers.Where(p => p.UserType == "Patient").ToListAsync();

            if (PatientProfiles != null)
            {
                return Ok(new
                {
                    PatientProfiles,
                    message = "Complete Patient List"
                });
            }
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
            else
            {
                return BadRequest(new
                {
                    response = 301,
<<<<<<< HEAD
                    message = "Invalid Patient Email Supplied"
=======
                    message = "Invalid Credentials Passed"
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
                });
            }
        }

<<<<<<< HEAD
        [Route("GetPatientQueue")]
        [HttpGet]
        public async Task<IActionResult> GetPatientQueueAsync()
        {
            var PatientQueue = await _applicationDbContext.PatientQueue.Where(p => p.DateOfConsultation.Date == DateTime.Today)
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
=======
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

>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
<<<<<<< HEAD
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

=======
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
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
    }
}
