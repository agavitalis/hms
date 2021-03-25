using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
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
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IDrug _drug;

        public MedicationController(IMedication medication, IAdmission admission, IMapper mapper, IAdmissionInvoice admissionInvoice, IDrug drug)
        {
            _admission = admission;
            _admissionInvoice = admissionInvoice;
            _drug = drug;
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

        [Route("AdministerMedication")]
        [HttpPost]
        public async Task<IActionResult> AdministerMedication([FromBody] MedicationDtoForAdminister Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var admission = await _admission.GetAdmission(Medication.AdmissionId);
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(Medication.AdmissionId);

            //update admission invoice price for request
            if (admission == null)
            {
                return BadRequest(new { response = "301", message = "Invalid Admission Id passed" });
            }

            if (admissionInvoice == null)
            {
                return BadRequest(new { response = "301", message = "No Invoice For This Admission" });
            }
            var drug = await _drug.GetDrug(Medication.DrugId);

            if (drug == null)
            {
                return BadRequest(new { response = "301", message = "Invalid Drug Id" });
            }

            var invoiceId = await _admissionInvoice.UpdateAdmissionInvoice(Medication, admissionInvoice);
            if (invoiceId == "1")
                return BadRequest(new
                {
                    response = "301",
                    message = "Out of Stock For This Drug"
                });


            //check if the admission exists
            
           
               
           

            var medicationToAdminister = _mapper.Map<AdmissionDrugDispensing>(Medication);

            var medication = await _medication.AdministerMedication(medicationToAdminister);
            if (!medication)
            {
                return BadRequest(new { response = "301", message = "Medication failed to Administer" });
            }

            
            return Ok(new
            {
                medicationToAdminister,
                message = "Medication Administered successfully"
            });
        }

    }
}
