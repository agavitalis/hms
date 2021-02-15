using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Medication")]
    [ApiController]
    public class MedicationController : Controller
    {
        private readonly IMedication _medication;
        private readonly IMapper _mapper;

        public MedicationController(IMedication medication, IMapper mapper)
        {
            _medication = medication;
            _mapper = mapper;
        }


        [Route("GetMedications")]
        [HttpGet]
        public async Task<IActionResult> GetMedications([FromQuery] PaginationParameter paginationParameter, string AdmissionId)
        {

            var medications = _medication.GetMedications(AdmissionId, paginationParameter);

            var paginationDetails = new
            {
                medications.TotalCount,
                medications.PageSize,
                medications.CurrentPage,
                medications.TotalPages,
                medications.HasNext,
                medications.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                medications,
                paginationDetails,
                message = "Admissions Fetched"
            });
        }


        [Route("CreateMedication")]
        [HttpPost]
        public async Task<IActionResult> CreateMedication([FromBody] MedicationDtoForCreate Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var medicationToCreate = _mapper.Map<AdmissionMedication>(Medication);

            var medication = await _medication.CreateMedication(medicationToCreate);
            if (!medication)
            {
                return BadRequest(new { response = "301", message = "Medication failed to create" });
            }

            return Ok(new
            {
                medicationToCreate,
                message = "Medication created successfully"
            });
        }

    }
}
