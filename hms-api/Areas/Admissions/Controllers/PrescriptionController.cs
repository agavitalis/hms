using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Prescriptions")]
    [ApiController]
    public class PrescriptionController : Controller
    {
        private readonly IPrescription _prescription;
        private readonly IUser _user;
        private readonly IMapper _mapper;

        public PrescriptionController(IPrescription prescription, IUser user, IMapper mapper)
        {
            _prescription = prescription;
            _user = user;
            _mapper = mapper;
        }

        [Route("GetPrescription")]
        [HttpGet]
        public async Task<IActionResult> GetAdmissionPrescription(string PrescriptionId)
        {
            if (PrescriptionId == "")
            {
                return BadRequest();
            }

            var prescription = await _prescription.GetAdmissionPrescription(PrescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            return Ok(new { prescription, message = "Prescription returned" });
        }


        [Route("GetPrescriptionsForAdmission")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptionsForAdmission(string AdmissionId, [FromQuery] PaginationParameter paginationParameter)
        {
            var prescriptions = _prescription.GetAdmissionPrescriptions(AdmissionId, paginationParameter);

            var paginationDetails = new
            {
                prescriptions.TotalCount,
                prescriptions.PageSize,
                prescriptions.CurrentPage,
                prescriptions.TotalPages,
                prescriptions.HasNext,
                prescriptions.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                prescriptions,
                paginationDetails,
                message = "Prescriptions Fetched"
            });
        }

        [Route("GetPrescriptions")]
        [HttpGet]
        public async Task<IActionResult> GetAdmissionPrescriptions([FromQuery] PaginationParameter paginationParameter)
        {
            var prescriptions = _prescription.GetAdmissionPrescriptions(paginationParameter);

            var paginationDetails = new
            {
                prescriptions.TotalCount,
                prescriptions.PageSize,
                prescriptions.CurrentPage,
                prescriptions.TotalPages,
                prescriptions.HasNext,
                prescriptions.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                prescriptions,
                paginationDetails,
                message = "Prescriptions Fetched"
            });
        }

        [Route("UpdatePrescription")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientVitals([FromBody] PrescriptionsDtoForUpdate prescriptions)
        {
            if (prescriptions == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var bedToCreate = _mapper.Map<AdmissionPrescription>(prescriptions);

            var res = await _prescription.UpdatePrescriptions(bedToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Prescription failed to create" });
            }

            return Ok(new
            {
                prescriptions,
                message = "Prescription created successfully"
            });
        }
    }
}
