﻿using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Surgery")]
    [ApiController]
   
    public class SurgeryController : Controller
    {
        private readonly IDoctorAppointment _appointment;
        private readonly IConsultation _consultation;
        private readonly ISurgery _surgery;
        private readonly IMapper _mapper;
        private readonly IUser _user;


        public SurgeryController(IDoctorAppointment appointment, IUser user, IConsultation consultation, ISurgery surgery, IMapper mapper)
        {
            _appointment = appointment;
            _consultation = consultation;
            _surgery = surgery;
            _user = user;
            _mapper = mapper;
        }

        [HttpGet("GetSurgery")]
        public async Task<IActionResult> GetSurgery(string SurgeryId)
        {
            var surgery = await _surgery.GetSurgery(SurgeryId);

            if (surgery == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                surgery,

                message = "Surgery Returned"
            });
        }


        [HttpGet("GetSurgeries")]
        public async Task<IActionResult> GetSurgeries([FromQuery] PaginationParameter paginationParameter)
        {
            var surgeries = _surgery.GetSurgeries(paginationParameter);

            var paginationDetails = new
            {
                surgeries.TotalCount,
                surgeries.PageSize,
                surgeries.CurrentPage,
                surgeries.TotalPages,
                surgeries.HasNext,
                surgeries.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                surgeries,
                paginationDetails,
                message = "Surgeries Returned"
            });
        }

        [HttpGet("GetSurgeriesByConsulatatonOrAppointment")]
        public async Task<IActionResult> GetSurgeriesByConsulatatonOrAppointment(string Id, [FromQuery] PaginationParameter paginationParameter)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);


            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var surgeries = _surgery.GetSurgeriesByAppointmentOrConsultation(Id, paginationParameter);

            var paginationDetails = new
            {
                surgeries.TotalCount,
                surgeries.PageSize,
                surgeries.CurrentPage,
                surgeries.TotalPages,
                surgeries.HasNext,
                surgeries.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                surgeries,
                paginationDetails,
                message = "Consultations Fetched"
            });
        }

        [Route("CreateSurgery")]
        [HttpPost]
        public async Task<IActionResult> CreateSurgery(SurgeryDtoForCreate Surgery)
        {
            
            if (Surgery == null)
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var surgery = false;
            var consultation = await _consultation.GetConsultationById(Surgery.Id);
            var appointment = await _appointment.GetAppointmentById(Surgery.Id);
            var initiator = await _user.GetUserByIdAsync(Surgery.InitiatorId);

            if (initiator == null)
            {
                return BadRequest(new { message = "Invalid Initiator Id" });
            }
            if (Surgery.Id != null)
            {
                if (consultation == null && appointment == null)
                {
                    return BadRequest(new { message = "Invalid Id" });
                }

                if (consultation != null)
                {
                    surgery = await _surgery.CreateSurgery(Surgery.Id, Surgery.IdType, Surgery.InitiatorId, consultation.PatientId, Surgery.ReferralNote, Surgery.DateOfSurgery, Surgery.TimeOfSurgery);
                }
                if (appointment != null)
                {
                    surgery = await _surgery.CreateSurgery(Surgery.Id, Surgery.IdType, Surgery.InitiatorId, appointment.PatientId, Surgery.ReferralNote, Surgery.DateOfSurgery, Surgery.TimeOfSurgery);
                }
            }
            else
            {
                surgery = await _surgery.CreateSurgery(Surgery.InitiatorId, Surgery.PatientId, Surgery.ReferralNote, Surgery.DateOfSurgery, Surgery.TimeOfSurgery);
            }
            
            if (surgery)
            {
                return Ok(new { message = "Surgery has been booked" });
            }
            else
            {
                return BadRequest(new { message = "Something went wrong" });
            }
        }
      

       



        [HttpPost("UpdateOperationNoteOne")]
        public async Task<IActionResult> UpdateOperationNoteOne(OperationNoteOneDtoForUpdate surgery)
        {
            if (surgery == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var surgeryToUpdate = _mapper.Map<Surgery>(surgery);
            var res = await _surgery.UpdateSurgery(surgeryToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Surgery failed to update" });
            }

            return Ok(new
            {
                surgery,
                message = "Surgery updated successfully"
            });
        }

        [HttpPost("UpdateOperationNoteTwo")]
        public async Task<IActionResult> UpdateOperationNoteTwo(OperationNoteTwoDtoForUpdate surgery)
        {

            if (surgery == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var surgeryToUpdate = _mapper.Map<Surgery>(surgery);
            var res = await _surgery.UpdateSurgery(surgeryToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Surgery failed to update" });
            }

            return Ok(new
            {
                surgery,
                message = "Surgery updated successfully"
            });
        }

        [HttpPost("UpdateOperationProcedure")]
        public async Task<IActionResult> UpdateOperationProcedure(OperationProcedureDtoForUpdate surgery)
        {

            if (surgery == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var surgeryToUpdate = _mapper.Map<Surgery>(surgery);
            var res = await _surgery.UpdateSurgery(surgeryToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Surgery failed to update" });
            }

            return Ok(new
            {
                surgery,
                message = "Surgery updated successfully"
            });
        }
    }
}
