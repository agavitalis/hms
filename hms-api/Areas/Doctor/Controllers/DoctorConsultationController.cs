using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Consultation")]
    [ApiController]
    public class DoctorConsultationController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IDoctorConsultation _consultation;
        public DoctorConsultationController(IDoctorConsultation consultation)
        {
            _consultation = consultation;
        }

       

        [Route("GetConsultationsOnOpenList")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsOnOpenList(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsOnOpenList(DoctorId, paginationParameter);

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

        [Route("GetConsultationsWithDoctor")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsWithDoctors(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsWithDoctor(DoctorId, paginationParameter);

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

        [Route("GetConsultationsCompleted")]
        [HttpGet]
        public async Task<IActionResult> GetPatientConsultationsCompleted(string DoctorId, [FromQuery] PaginationParameter paginationParameter)
        {
            var consultations = _consultation.GetConsultationsCompleted(DoctorId, paginationParameter);

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

    }
}
