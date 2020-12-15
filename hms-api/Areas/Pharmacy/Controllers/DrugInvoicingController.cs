using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            private readonly IMapper _mapper;
            private readonly IPatientProfile _patientRepo;

            public DrugInvoicingController(IDrugInvoicing drugInvoicing, IMapper mapper, IPatientProfile patientRepo)
            {
                 _drugInvoicing = drugInvoicing;
                _mapper = mapper;
                _patientRepo = patientRepo;
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


        //[HttpGet("GetDrugPrescriptionInvoicesForPatient/{patientId}")]
        //public async Task<IActionResult> GetPatientInvoice(string patientId)
        //{
        //    var patientInvoices = await _drugInvoicing.GetDrugPrescriptionInvoicesForPatient(patientId);
        //    if (!patientInvoices.Any())
        //        return Ok(new
        //        {
        //            response = "204",
        //            message = "No Service Request in this patient"
        //        });

        //    return Ok(new
        //    {
        //        patientInvoices,
        //        message = "List of request in invoice fetched"
        //    });
        //}


        //[HttpGet("GetDrugPrescription/{DrugPrescriptionId}")]
        //public async Task<IActionResult> GetDrugPrescription(string DrugPrescriptionId)
        //{
        //    var drugCosting = await _drugInvoicing.GetDrugPrescription(DrugPrescriptionId);
        //    if (drugCosting == null)
        //    {
        //        return BadRequest(new { message = "Invalid post attempt" });
        //    }

        //    return Ok(new
        //    {
        //        drugCosting,
        //        message = "Drug Prescription"
        //    });
        //}


        //[HttpPost("PayForPrescription")]
        //public async Task<IActionResult> PayForServices(DrugPrescriptionPaymentDto drugs)
        //{
        //    if (drugs == null)
        //    {
        //        return BadRequest(new { message = "Invalid Post attempt made" });
        //    }

        //    //check if the patient exists
        //    var patient = await _patientRepo.GetPatientByIdAsync(drugs.PatientId);
        //    if (patient == null)
        //        return BadRequest(new
        //        {
        //            response = "301",
        //            message = "Invalid patient Id passed, Patient not found",
        //        });

        //    //check if all serviceRequest id passed exist
        //    var drugPrescriptionCheck = await _drugPrescription.CheckIfDrugPrescriptionIdExists(drugs.DrugPrescriptionId);
        //    if (!drugPrescriptionCheck)
        //        return BadRequest(new
        //        {
        //            response = "301",
        //            message = "One or more Service Request Id Passed is/are invalid"
        //        });

        //    //check if the amount is correct
        //    var correctAmount = await _drugPrescription.CheckIfAmountPaidIsCorrect(drugs);
        //    if (correctAmount == false)
        //        return BadRequest(new
        //        {
        //            response = "301",
        //            message = "The Amount Paid and the drug prescription paid for does not match"
        //        });

        //    //pay for services
        //    var result = await _drugPrescription.PayForServices(drugs);
        //    if (!result)
        //        return BadRequest(new
        //        {
        //            response = "301",
        //            message = "Payment for these drugs cannot be completed, pls contact the Admins"
        //        });

        //    return Ok(new { message = "Payment for drugs completed successfully" });
        //}
    }
}

