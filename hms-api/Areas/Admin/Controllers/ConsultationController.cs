using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Consultations")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultation _consultation;
        private readonly IMapper _mapper;
        private readonly IUser _userRepo;
        private readonly IDoctor _doctor;
        private readonly IDoctorClerking _clerking;
   


        public ConsultationController(IConsultation consultation, IMapper mapper, IUser userRepo, IDoctor doctor, IDoctorClerking clerking)
        {
            _consultation = consultation;
            _mapper = mapper;
            _userRepo = userRepo;
            _doctor = doctor;
            _clerking = clerking;
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

        [Route("ReassignPatientToAnotherDoctor")]
        [HttpPatch]
        public async Task<IActionResult> ReassignPatientToAnotherDoctor(string ConsultationId, JsonPatchDocument<ConsultationDtoForUpdate> Consultation)
        {
            var consultation = await _consultation.GetConsultationById(ConsultationId);
            var doctorId = Consultation.Operations[0].value.ToString();
            var doctor = await _doctor.GetDoctorsById(doctorId);
        
            if (consultation == null || Consultation == null || doctor == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }


            //then we patch
            await _consultation.ReassignPatientToNewDoctor(consultation, Consultation);

            return Ok(new
            {
                consultation,
                message = "Patient Reassigned Successfully"
            });
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

       

        [Route("DeleteConsultation")]
        [HttpPost]
        public async Task<IActionResult> DeleteConsultation(ConsultationDtoForDelete Consultation)
        {
            //check if this guy has a profile already
            var consultation = await _consultation.GetConsultationById(Consultation.ConsultationId);

            if (consultation == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Consultation Id" });
            }

            var clerking = await _clerking.GetDoctorClerkingByConsultation(Consultation.ConsultationId);

            if (clerking != null)
            {
                return BadRequest(new { response = 301, message = "This Consultation Has An Associated Clerking And Cannot Be Deleted" });
            }

            await _consultation.DeleteConsultation(consultation);

            return Ok(new { message = "Consultation Successfully Deleted" });
        }

    }
}
