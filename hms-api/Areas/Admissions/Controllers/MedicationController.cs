using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
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
        private readonly IServices _service;

        public MedicationController(IMedication medication, IAdmission admission, IMapper mapper, IAdmissionInvoice admissionInvoice, IDrug drug, IServices service)
        {
            _admission = admission;
            _admissionInvoice = admissionInvoice;
            _drug = drug;
            _service = service;
            _medication = medication;
            _mapper = mapper;
        }


        [Route("GetDrugMedications")]
        [HttpGet]
        public async Task<IActionResult> GetMedications([FromQuery] PaginationParameter paginationParameter, string AdmissionId)
        {

            var medications = _medication.GetDrugMedications(AdmissionId, paginationParameter);

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


        [Route("CreateDrugMedication")]
        [HttpPost]
        public async Task<IActionResult> CreateMedication([FromBody] DrugMedicationDtoForCreate Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var medicationToCreate = _mapper.Map<AdmissionDrugMedication>(Medication);

            var medication = await _medication.CreateDrugMedication(medicationToCreate);
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


        [Route("UpdateDrugMedicationStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateMedicationStatus([FromBody] DrugMedicationStatusDtoForUpdate Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var medication = await _medication.GetDrugMedication(Medication.MedicationId);
            medication.Status = Medication.Status;

            var medicationToUpdate = await _medication.UpdateDrugMedication(medication);
            if (!medicationToUpdate)
            {
                return BadRequest(new { response = "301", message = "Medication failed to Update" });
            }

            return Ok(new
            {
                medication,
                message = "Medication created successfully"
            });
        }


        [Route("AdministerDrugMedication")]
        [HttpPost]
        public async Task<IActionResult> AdministerMedication([FromBody] DrugMedicationDtoForAdminister Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            //check if the admission exists
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
            {
                return BadRequest(new { response = "301", message = "Out of Stock For This Drug" });
            }
                
            var medicationToAdminister = _mapper.Map<AdmissionDrugDispensing>(Medication);
            medicationToAdminister.AdmissionInvoiceId = invoiceId;
            var medication = await _medication.AdministerDrugMedication(medicationToAdminister);
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


        [Route("GetServiceMedications")]
        [HttpGet]
        public async Task<IActionResult> GetServiceMedications([FromQuery] PaginationParameter paginationParameter, string AdmissionId)
        {

            var medications = _medication.GetServiceMedications(AdmissionId, paginationParameter);

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

        [Route("CreateServiceMedication")]
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest([FromBody] ServiceMedicationDtoForCreate ServiceRequest)
        {
            if (ServiceRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var serviceRequestToCreate = _mapper.Map<AdmissionServiceMedication>(ServiceRequest);
            var result = await _medication.CreateServiceMedication(serviceRequestToCreate);

            if (!result)
            {
                return BadRequest(new { response = "301", message = "Service Request Failed To Create" });
            }

            return Ok(new
            {
                serviceRequestToCreate,
                message = "Service Request Created Successfully"
            });
        }

        [Route("UpdateServiceMedicationStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateServiceMedicationStatus([FromBody] ServiceMedicationStatusDtoForUpdate Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var medication = await _medication.GetServiceMedication(Medication.MedicationId);
            medication.Status = Medication.Status;

            var medicationToUpdate = await _medication.UpdateServiceMedication(medication);
            if (!medicationToUpdate)
            {
                return BadRequest(new { response = "301", message = "Medication failed to Update" });
            }

            return Ok(new
            {
                medication,
                message = "Medication Updated successfully"
            });
        }


        [Route("AdministerServiceMedication")]
        [HttpPost]
        public async Task<IActionResult> AdministerServiceMedication([FromBody] ServiceMedicationDtoForAdminister Medication)
        {
            if (Medication == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            //check if the admission exists
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
            var service = await _service.GetServiceByIdAsync(Medication.ServiceId);

            if (service == null)
            {
                return BadRequest(new { response = "301", message = "Invalid Drug Id" });
            }

            var invoiceId = await _admissionInvoice.UpdateAdmissionInvoice(Medication, admissionInvoice);
            if (invoiceId == "1")
            {
                return BadRequest(new { response = "301", message = "Out of Stock For This Service" });
            }

            var medicationToAdminister = _mapper.Map<AdmissionServiceRequest>(Medication);
            medicationToAdminister.AdmissionInvoiceId = invoiceId;
            var medication = await _medication.AdministerServiceMedication(medicationToAdminister);
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
