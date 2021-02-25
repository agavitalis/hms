using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Medication Dispensing")]
    [ApiController]
    public class MedicationDispensingController : Controller
    {
        private readonly IMedicationDispensing _medicationDispensing;
        private readonly IMapper _mapper;

        public MedicationDispensingController(IMedicationDispensing medicationDispensing, IMapper mapper)
        {
            _medicationDispensing = medicationDispensing;
            _mapper = mapper;
        }

        [Route("GetMedicationDispensing")]
        [HttpGet]
        public async Task<IActionResult> GetPatientObservationChart(string AdmissionId)
        {
            var medicationDispensing = await _medicationDispensing.GetAdmissionMedicationDispensing(AdmissionId);

            if (medicationDispensing == null)
            {
                return BadRequest(new { message = "This Admission Has No Observation Chart History" });
            }

            return Ok(new
            {
                medicationDispensing,
                message = "Medication Dispensing Fetched"
            });
        }


        [Route("CreateMedicationDispensing")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicationDispensing([FromBody] AdmissionMedicationDispensingDtoForCreate MedicationDispensing)
        {
            if (MedicationDispensing == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var medicationDispensingToCreate = _mapper.Map<AdmissionMedicationDispensing>(MedicationDispensing);

            var medicationDispensing = await _medicationDispensing.CreateMedicationDispensing(medicationDispensingToCreate);
            if (!medicationDispensing)
            {
                return BadRequest(new { response = "301", message = "Ward failed to create" });
            }

            return Ok(new
            {
                medicationDispensingToCreate,
                message = "Medication Dispensing Created"
            });
        }
    }
}
