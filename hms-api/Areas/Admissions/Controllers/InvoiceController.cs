using System;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Invoices")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmission _admission;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patient;
        private readonly ITransactionLog _transaction;


        public InvoiceController(IAdmissionInvoice admissionInvoice, IPatientProfile patient, IAdmission admission, ITransactionLog transaction, IMapper mapper)
        {
            _admissionInvoice = admissionInvoice;
            _patient = patient;
            _admission = admission;
            _mapper = mapper;
            _transaction = transaction;
        }

        [HttpGet("GetAdmissionTransactions")]
        public async Task<IActionResult> GetAccountTransactions([FromQuery] PaginationParameter paginationParameter, string AdmissionId)
        {
            var admission = await _admission.GetAdmission(AdmissionId);

            if (admission == null)
            {
                return BadRequest(new { message = "An Admission with this Id was not found" });
            }
            var admissionTransactions = _transaction.GetAdmissionTransactions(AdmissionId, paginationParameter);

            var paginationDetails = new
            {
                admissionTransactions.TotalCount,
                admissionTransactions.PageSize,
                admissionTransactions.CurrentPage,
                admissionTransactions.TotalPages,
                admissionTransactions.HasNext,
                admissionTransactions.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                admissionTransactions,
                paginationDetails,
                message = "Admission Transactions"
            });
        }

        [Route("GetAdmissionInvoice")]
        [HttpGet]
        public async Task<IActionResult> GetInvoiceForAdmission(string AdmissionId)
        {
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(AdmissionId);

            return Ok(new
            {
                admissionInvoice,
                message = "Admission Invoice Returned"
            });
        }

        [Route("PayForAdmission")]
        [HttpPost]
        public async Task<IActionResult> PayForAdmission(AdmissionPaymentDto Admission)
        {
            if (Admission == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var admission = await _admission.GetAdmission(Admission.AdmissionId);

            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid patient Id passed, Patient not found",
                });

            try
            {
                if (await _admissionInvoice.CheckIfAmountPaidIsCorrect(Admission))
                {
                    var result = await _admissionInvoice.PayForAdmission(Admission);
                    if (!result)
                        return BadRequest(new
                        {
                            response = "301",
                            message = "Amount Paid Would Be Greater Than Amount You Are Expected To Pay If Payment Is Completed"
                        });

                    return Ok(new { message = "Payment for Admission completed successfully" });
                }
                else
                {
                    return BadRequest(new { response = "301", message = "Amount Paid Will Be Greater Than Amount Due If Payment Is Completed" });
                }
               
            }
            catch (Exception e)
            {

                return BadRequest(new { message = e.Message.ToString() }); ;
            }

        }


        [Route("PayForAdmissionWithAccount")]
        [HttpPost]
        public async Task<IActionResult> PayForAdmissionServicesWithAccount(AdmissionPaymentDto Admission)
        {
            if (Admission == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var admission = await _admission.GetAdmission(Admission.AdmissionId);
            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid patient Id passed, Patient not found",
                });

            if (await _admissionInvoice.CheckIfAmountPaidIsCorrect(Admission))
            {
                var result = await _admissionInvoice.PayForAdmissionWithAccount(Admission);
                if (!result)
                {
                    return BadRequest(new { response = "301", message = "Account Balance Is Less Than The Amount Specified" });
                }
                
                return Ok(new { message = "Payment for services completed successfully" });
            }
            else
            {
                return BadRequest(new { response = "301", message = "Amount Paid Will Be Greater Than Amount Due If Payment Is Completed" });
            }           
        }
    }
}
