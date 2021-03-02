using System;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using Microsoft.AspNetCore.Mvc;

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


        public InvoiceController(IAdmissionInvoice admissionInvoice, IPatientProfile patient, IAdmission admission, IMapper mapper)
        {
            _admissionInvoice = admissionInvoice;
            _patient = patient;
            _admission = admission;
            _mapper = mapper;
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

           

            //check if the amount is correct
            var correctAmount = await _admissionInvoice.CheckIfAmountPaidIsCorrect(Admission);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid does not atch with the required amount"
                });

            //pay for services
            try
            {
                var result = await _admissionInvoice.PayForAdmission(Admission);
                if (!result)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "Payment for Admission cannot be completed, pls contact the Admins"
                    });

                return Ok(new { message = "Payment for Admission completed successfully" });
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

           

            //check if the amount is correct
            var correctAmount = await _admissionInvoice.CheckIfAmountPaidIsCorrect(Admission);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the services paid for does not match"
                });



            //pay for services
            var result = await _admissionInvoice.PayForAdmissionWithAccount(Admission);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Payment for these servies cannot be completed, pls contact the Admins"
                });

            return Ok(new { message = "Payment for services completed successfully" });
        }
    }
}
