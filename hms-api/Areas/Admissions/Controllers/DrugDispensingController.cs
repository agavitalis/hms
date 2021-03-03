using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Drug Dispensing")]
    [ApiController]
    public class DrugDispensingController : Controller
    {
       
        private readonly IMapper _mapper;
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmissionDrugDispensing _admissionDrugDispensing;
        private readonly IDrugInvoicing _drugInvoicing;
        private readonly IDrug _drug;
        private readonly IPatientProfile _patient;


        public DrugDispensingController(IMapper mapper, IAdmission admission, IAdmissionInvoice admissionInvoice, IAdmissionDrugDispensing admissionDrugDispensing, IDrug drug, IDrugInvoicing drugInvoicing, IPatientProfile patient)
        {
            _mapper = mapper;
            _admission = admission;
            _drug = drug;
            _admissionInvoice = admissionInvoice;
            _drugInvoicing = drugInvoicing;
            _patient = patient;
            _admissionDrugDispensing = admissionDrugDispensing;
        }

        [HttpPost("RequestDrug")]
        public async Task<IActionResult> RequestDrug(AdmissionDrugDispensingDtoForCreate AdmissionRequest)
        {
            if (AdmissionRequest == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            //check if the admission exists
            var admission = await _admission.GetAdmission(AdmissionRequest.AdmissionId);
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(AdmissionRequest.AdmissionId);
            if (admission == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid Admission Id passed",
                });

            //check if all drugs id passed exist
            if (AdmissionRequest.Drugs != null)
            {
                var drugCheck = await _drugInvoicing.CheckIfDrugsExist(AdmissionRequest.Drugs);
                if (!drugCheck)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "One or more Drugs Passed is/are invalid"
                    });

            }

            //update admission invoice price for request
            var invoiceId = await _admissionInvoice.UpdateAdmissionInvoice(AdmissionRequest, admissionInvoice);
            if (string.IsNullOrEmpty(invoiceId))
                return BadRequest(new
                {
                    response = "301",
                    message = "Failed to update invoice !!!, Try Again"
                });

            //insert request
            var result = await _admissionDrugDispensing.UpdateDrugDispensing(AdmissionRequest, admissionInvoice);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Request Service Failed !!!, Try Again"
                });

            return Ok(new { message = "Admission Request submitted successfully" });
        }

        [HttpGet("GetDrugsInAnInvoice")]
        public async Task<IActionResult> GetDrugsInAnInvoice(string invoiceId, [FromQuery] PaginationParameter paginationParameter)
        {
            var drugsInInvoice = _admissionDrugDispensing.GetAdmissionDrugDispensing(invoiceId, paginationParameter);
           
            var paginationDetails = new
            {
                drugsInInvoice.TotalCount,
                drugsInInvoice.PageSize,
                drugsInInvoice.CurrentPage,
                drugsInInvoice.TotalPages,
                drugsInInvoice.HasNext,
                drugsInInvoice.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                drugsInInvoice,
                paginationDetails,
                message = "Drug Dispensing Returned"
            });
        }


        [HttpPost("PayForDrugs")]
        public async Task<IActionResult> PayForServices(DrugDispensingPaymentDto drug)
        {
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var admission = await _admission.GetAdmission(drug.AdmissionId);
            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);
           
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "This given patient could not be found",
                });

            //check if the amount is correct
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(drug.AdmissionId);
            var correctAmount = await _admissionDrugDispensing.CheckIfAmountPaidIsCorrect(admissionInvoice.Id, drug.TotalAmount);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the Sum on the given invoice does not"
                });

            //pay for drugs
            var result = await _admissionDrugDispensing.PayForDrugs(drug);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Quantity in Stock is Less Than The Amount of Drugs Requested"
                });

            return Ok(new { message = "Payment for drugs completed successfully" });
        }

        [HttpPost("PayForDrugsWithAccount")]
        public async Task<IActionResult> PayForDrugsWithAccount(DrugDispensingPaymentDto drug)
        {
            if (drug == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var admission = await _admission.GetAdmission(drug.AdmissionId);
            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);

            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "This given patient could not be found",
                });

            //check if the amount is correct
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(drug.AdmissionId);
            var correctAmount = await _admissionDrugDispensing.CheckIfAmountPaidIsCorrect(admissionInvoice.Id, drug.TotalAmount);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the Sum on the given invoice does not"
                });

            //check if the account balance is greater than amount to be paid
            if (patient.Account.AccountBalance < drug.TotalAmount)
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "Account Balance Is Less Than Amount Specified"
                });
            }

            //pay for drugs
            var result = await _admissionDrugDispensing.PayForDrugsWithAccount(drug);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Payment for these drugs cannot be completed, pls contact the Admins"
                });

            return Ok(new { message = "Payment for drugs completed successfully" });
        }

        //[HttpPost("MarkInvoiceAsDispensed")]
        //public async Task<IActionResult> MarkInvoiceAsDispensed(string DrugInvoiceId)
        //{
        //    var drugInvoice = await _drugInvoicing.GetDrugDispencingInvoice(DrugInvoiceId);

        //    if (drugInvoice == null)
        //    {
        //        return BadRequest(new { response = "301", message = "Invalid Drug Invoice Id" });
        //    }
        //    drugInvoice.IsDispensed = true;

        //    var response = await _drugInvoicing.UpdateDrugInvoice(drugInvoice);
        //    if (!response)
        //    {
        //        return BadRequest(new
        //        {
        //            response = "301",
        //            message = "There was an error"
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new { message = "Drug Marked as Dispensed" });
        //    };
        //}
    }
}
