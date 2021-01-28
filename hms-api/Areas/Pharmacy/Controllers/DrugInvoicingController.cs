﻿using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy - Drug Invoicing and Payments")]
    [ApiController]
    public class DrugInvoicingController : ControllerBase
    {
        private readonly IDrugInvoicing _drugInvoicing;
        private readonly IPatientProfile _patientRepo;
        private readonly IDoctorClerking _clerking;

        public DrugInvoicingController(IDrugInvoicing drugInvoicing, IPatientProfile patientRepo, IDoctorClerking clerking)
        {
            _drugInvoicing = drugInvoicing;
            _patientRepo = patientRepo;
            _clerking = clerking;
        }

        [Route("GetPrescriptions")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptions()
        {
            var clerkings = await _clerking.GetClerkings();

            var prescriptions = clerkings
             .Select(p => new
             {
                 Id = p.Id,
                 Prescription = p.Prescription,
                 Doctor = p.Doctor,
                 Patient = p.Patient,
                 DatePrescribed = p.DateOfClerking
             });

            return Ok(new
            {
                prescriptions,
                message = "Prescriptions Returned"
            });
        }

        [Route("GetPrescription")]
        [HttpGet]
        public async Task<IActionResult> GetPrescription(string ClerkingId)
        {
            var clerking = await _clerking.GetClerking(ClerkingId);
            if (clerking == null)
            {
                return BadRequest(new
                {
                    message = "Invalid Clarking Id passed"
                });
            }
            var Id = clerking.Id;
            var prescription = clerking.Prescription;
            var doctor = clerking.Doctor;
            var patient = clerking.Patient;
            var datePrescribed = clerking.DateOfClerking;

            return Ok(new
            {
                Id,
                prescription,
                doctor,
                patient,
                datePrescribed,
                message = "Prescription Returned"
            });
        }

        [HttpPost("GenerateDrugDispenseInvoice")]
        public async Task<IActionResult> GenerateDrugDispenseInvoice(DrugInvoicingDto DrugInvoicing)
        {
            if (DrugInvoicing == null)
            {
                return BadRequest(new { message = "Please fill all the required parameters" });
            }

            //check if the patient exist
            var patient = await _patientRepo.GetPatientByIdAsync(DrugInvoicing.PatientId);
            if (patient == null)
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid Patient Credentials Passed, Patient not found",
                });
            }

            //check if all drugs id passed exist
            var drugCheck = await _drugInvoicing.CheckIfDrugsExist(DrugInvoicing.Drugs);
            if (!drugCheck)
                return BadRequest(new
                {
                    response = "301",
                    message = "One or more Drugs Passed is/are invalid"
                });

            //Generate Invoice for these drugs 
            var invoiceId = await _drugInvoicing.GenerateDrugDispenseInvoice(DrugInvoicing);
            if (invoiceId == "1")
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "Quantity in Stock is Less Than The Amount of Drugs Requested"
                });
            }
            var dispensingList = await _drugInvoicing.CreateDespenseRequest(DrugInvoicing, invoiceId);

            return Ok(new
            {
                dispensingList,
                response = "200",
                message = "Drug Dispense Invoice Was Successfully Generated"
            });

        }


        [HttpGet("GetDrugDispencingInvoices")]
        public async Task<IActionResult> GetAllServiceInvoice()
        {
            var drugInvoices = await _drugInvoicing.GetDrugDispencingInvoices();

            return Ok(new
            {
                drugInvoices,
                message = "List of invoice fetched"
            });
        }


        [HttpGet("GetDrugsInAnInvoice/{invoiceNumber}")]
        public async Task<IActionResult> GetDrugsInAnInvoice(string invoiceNumber)
        {
            var drugsInInvoice = await _drugInvoicing.GetDrugsInAnInvoice(invoiceNumber);
            if (!drugsInInvoice.Any())
                return BadRequest(new
                {
                    response = "400",
                    message = "No Drug Despensed With this invoice number"
                });

            return Ok(new
            {
                drugsInInvoice,
                message = "List of Drugs in this given invoice"
            });
        }


        [HttpGet("GetPatientDrugInvoices/{patientId}")]
        public async Task<IActionResult> GetPatientDrugInvoices(string patientId)
        {
            var patientInvoices = await _drugInvoicing.GetPatientDrugInvoices(patientId);
            if (!patientInvoices.Any())
                return Ok(new
                {
                    response = "204",
                    message = "No Service Request in this patient"
                });

            return Ok(new
            {
                patientInvoices,
                message = "List of request in invoice fetched"
            });
        }


        [HttpPost("PayForDrugs")]
        public async Task<IActionResult> PayForServices(DrugInvoicingPaymentDto drugInvoice)
        {
            if (drugInvoice == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var patient = await _patientRepo.GetPatientByIdAsync(drugInvoice.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "This given patient could not be found",
                });

            //check if the amount is correct
            var correctAmount = await _drugInvoicing.CheckIfAmountPaidIsCorrect(drugInvoice.InvoiceNumber, drugInvoice.TotalAmount);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the Sum on the given invoice does not"
                });

            //pay for drugs
            var result = await _drugInvoicing.PayForDrugs(drugInvoice);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Quantity in Stock is Less Than The Amount of Drugs Requested"
                });

            return Ok(new { message = "Payment for drugs completed successfully" });
        }

        [HttpPost("PayForDrugsWithAccount")]
        public async Task<IActionResult> PayForDrugsWithAccount(DrugInvoicingPaymentDto drugInvoice)
        {
            if (drugInvoice == null)
            {
                return BadRequest(new { message = "Invalid Post attempt made" });
            }

            //check if the patient exists
            var patient = await _patientRepo.GetPatientByIdAsync(drugInvoice.PatientId);
            if (patient == null)
                return BadRequest(new
                {
                    response = "301",
                    message = "This given patient could not be found",
                });

            //check if the amount is correct
            var correctAmount = await _drugInvoicing.CheckIfAmountPaidIsCorrect(drugInvoice.InvoiceNumber, drugInvoice.TotalAmount);
            if (correctAmount == false)
                return BadRequest(new
                {
                    response = "301",
                    message = "The Amount Paid and the Sum on the given invoice does not"
                });

            //check if the account balance is greater than amount to be paid
            if (patient.Account.AccountBalance < drugInvoice.TotalAmount)
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "Account Balance Is Less Than Amount Specified"
                });
            }

            //pay for drugs
            var result = await _drugInvoicing.PayForDrugsWithAccount(drugInvoice);
            if (!result)
                return BadRequest(new
                {
                    response = "301",
                    message = "Payment for these drugs cannot be completed, pls contact the Admins"
                });
            
            return Ok(new { message = "Payment for drugs completed successfully" });
        }

        [HttpPost("MarkInvoiceAsDispensed")]
        public async Task<IActionResult> MarkInvoiceAsDispensed(string DrugInvoiceId)
        {
            var drugInvoice = await _drugInvoicing.GetDrugDispencingInvoice(DrugInvoiceId);

            if (drugInvoice == null)
            {
                return BadRequest(new { response = "301", message = "Invalid Drug Invoice Id" });
            }
            drugInvoice.IsDispensed = true;

            var response = await _drugInvoicing.UpdateDrugInvoice(drugInvoice);
            if (!response)
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "There was an error"
                });
            }
            else 
            {
                return Ok(new { message = "Drug Marked as Dispensed" });
            };
        }
    }
}

