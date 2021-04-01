﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        private readonly IDoctorClerking _clerking;
      


        public ConsultationController(IConsultation consultation, IMapper mapper, IUser userRepo, IDoctorClerking clerking)
        {
            _consultation = consultation;
            _mapper = mapper;
            _userRepo = userRepo;
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

        [Route("GetPatientsOnOpenListCount")]
        [HttpGet]
        public async Task<IActionResult> GetPatientsOnOpenListCount()
        {
            var consultationCount = await _consultation.GetPatientsOnOpenListCount();

            return Ok(new
            {
                consultationCount,
                message = "Consultaion Open List Count"
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
            var doctor = await _userRepo.GetUserByIdAsync(consultation.DoctorId);
            var doctorPatient = await _consultation.CheckDoctorInMyPatients(consultation.DoctorId, consultation.PatientId);
            if (patient == null)
            {
                return BadRequest(new { response = 301, message = "Invalid Patient Id" });
            }

            
            var consultationToBook = _mapper.Map<Consultation>(consultation);





            var res = await _consultation.BookConsultation(consultationToBook);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Book Consultation" });
            }
            else
            {
                if (doctorPatient == null)
                {
                    var myPatient = new MyPatient();

                    myPatient = new MyPatient()
                    {
                        DoctorId = consultation.DoctorId,
                        PatientId = consultation.PatientId,
                        DateCreated = DateTime.Now
                    };

                    var result = await _consultation.AssignDoctorToPatient(myPatient);
                    if (result)
                    {
                        return Ok(new { message = "Consultation Successfully Booked" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed To Assign Patient To Doctor" });
                    }
                }
                else
                {
                    return Ok(new { message = "Consultation Successfully Booked" });
                }
            }
        }


        [Route("ReassignPatientToAnotherDoctor")]
        [HttpPost]
        public async Task<IActionResult> ReassignAppointment(ReassignConsultationDto Consultation)
        {
            //check if this guy has a profile already
            var consultation = await _consultation.GetConsultationById(Consultation.ConsultationId);
            var doctor = await _userRepo.GetUserByIdAsync(Consultation.DoctorId);
            var doctorPatient = await _consultation.CheckDoctorInMyPatients(Consultation.DoctorId, consultation.PatientId);
            // Validate patient is not null---has no profile yet
            if (consultation != null && doctor != null)
            {
               
                //if its avaliable now book it
                var doctorConsultation = _mapper.Map<Consultation>(consultation);
                doctorConsultation.DoctorId = Consultation.DoctorId;
                var res = await _consultation.UpdateConsultation(doctorConsultation);

                if (!res)
                {
                    return BadRequest(new { message = "failed to book consultation" });
                }
                else
                {
                    if (doctorPatient == null)
                    {
                        var myPatient = new MyPatient();

                        myPatient = new MyPatient()
                        {
                            DoctorId = consultation.DoctorId,
                            PatientId = consultation.PatientId,
                            DateCreated = DateTime.Now
                        };

                        var result = await _consultation.AssignDoctorToPatient(myPatient);
                        if (result)
                        {
                            return Ok(new { message = "Consultation Successfully Booked" });
                        }
                        else
                        {
                            return BadRequest(new { message = "Failed To Assign Patient To Doctor" });
                        }
                    }
                    else
                    {
                        return Ok(new { message = "Consultation Successfully Booked" });
                    }
                }
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Appointment Id or Doctor Id Supplied"
                });
            }
        }

        //[Route("GetPatientConsultations")]
        //[HttpGet]
        //public async Task<IActionResult> GetConsultations()
        //{
        //    var consultations = await _consultation.GetConsultations();
        //    return Ok(new { consultations, message = "Consultation List" });
        //}

        [Route("GetPatientConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetConsultations([FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsPagination(paginationParameter);
            
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
                message = "Consultations Fetched"
            });
        }


        [Route("GetPatientConsultationsOnOpenList")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsOnOpenList([FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsOnOpenList(paginationParameter);

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
                message = "Consultations Fetched"
            });
        }

        [Route("GetPatientConsultationsWithDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsWithDoctors([FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsWithDoctors(paginationParameter);

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
                message = "Consultations Fetched"
            });
        }

        [Route("GetPatientConsultationsCompleted")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsCompleted([FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsCompleted(paginationParameter);

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
                message = "Consultations Fetched"
            });
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
