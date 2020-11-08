using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
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
        private readonly IConsultation _consultation;
        private readonly IMapper _mapper;
        private readonly IUser _userRepo;


        public ConsultationController(IConsultation consultation, IMapper mapper, IUser userRepo)
        {
            _consultation = consultation;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        [Route("GetPatientConsultationCount")]
        [HttpGet]
        public async Task<IActionResult> GetConsultationCount()
        {
            var consultationCount = await _consultation.GetConsultationCount();
            
            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetPatientsUnattendedToCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsUnattendedToCount()
        {
            var consultationCount = await _consultation.GetPatientsUnattendedToCount();

            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("GetPatientsAttendedToCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsAttendedToCount()
        {
            var consultationCount = await _consultation.GetPatientsAttendedToCount();

            return Ok(new
            {
                consultationCount,
                message = "Patient Consultation Queue Count"
            });
        }

        [Route("BookConsultation")]
        [HttpPost]
        public async Task<IActionResult> BookConsultation([FromBody] BookConsultation consultation)
        {
            
            if (consultation == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var patient = await _userRepo.GetUserByIdAsync(consultation.PatientId);
            
            if (patient != null)
            {
                var consultationToBook = _mapper.Map<Consultation>(consultation);

                var res = await _consultation.BookConsultation(consultationToBook);
                if (!res)
                {
                    return BadRequest(new { response = "301", message = "Failed To Book Consultation" });
                }

                return Ok(new
                {
                    consultation,
                    message = "Consultation successfully booked"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Patient Id or Doctor Id Supplied"
                });
            }
        }

        [Route("GetPatientConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetConsultations()
        {
            var consultations = await _consultation.GetConsultations();
            return Ok(new { consultations, message = "Consultation List" });
        }


        [Route("CancelConsultation")]
        [HttpPatch]
        public async Task<IActionResult> CancelConsultation(string consultationId)
        {
            var response = await _consultation.CancelPatientConsultationAsync(consultationId);

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
            var response = await _consultation.ExpirePatientConsultationAsync(consultationId);
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
            var response = await _consultation.CompletePatientConsultationAsync(consultationId);
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
