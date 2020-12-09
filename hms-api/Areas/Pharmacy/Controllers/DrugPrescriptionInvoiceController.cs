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
    public class DrugPrescriptionInvoiceController : Controller
    {
        [Route("api/Admin", Name = "Admin - Manage Service Requests")]
        [ApiController]
        public class ServiceRequestController : ControllerBase
        {
            private readonly IDrugPrescriptionInvoice _drugPrescription;
            private readonly IMapper _mapper;
            private readonly IPatientProfile _patientRepo;

            public ServiceRequestController(IDrugPrescriptionInvoice drugPrescription, IMapper mapper, IPatientProfile patientRepo)
            {
                _drugPrescription = drugPrescription;
                _mapper = mapper;
                _patientRepo = patientRepo;
            }


            [HttpPost("GenerateDrugPrscriptionInvoice")]
            public async Task<IActionResult> GenerateDrugPrscriptionInvoice(DrugPrescriptionInvoiceDtoForCreate DrugPrescription)
            {
                if (DrugPrescription == null)
                {
                    return BadRequest(new { message = "Invalid post attempt" });
                }

                //check if the patient exist
                var patient = await _patientRepo.GetPatientByIdAsync(DrugPrescription.PatientId);
                if (patient == null)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "Invalid patient Id passed, Patient not found",
                    });

                //serviceRequest.PatientId = patient.PatientId;
                //check if all service id passed exist
                var drugCheck = await _drugPrescription.CheckIfDrugsExist(DrugPrescription.DrugId);
                if (!drugCheck)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "One or more drug id passed is/are invalid"
                    });

                //generate invoice for request
                var invoiceId = await _drugPrescription.GenerateInvoiceForDrugPrescription(DrugPrescription);
                if (string.IsNullOrEmpty(invoiceId))
                    return BadRequest(new
                    {
                        response = "301",
                        message = "Failed to generate invoice !!!, Try Again"
                    });

                //insert request
                var result = await _drugPrescription.CreateDrugPrescription(DrugPrescription, invoiceId);
                if (!result)
                    return BadRequest(new
                    {
                        response = "301",
                        message = "Drug Prescription Failed !!!, Try Again"
                    });

                return Ok(new { message = "Drug Prescription submitted successfully" });
            }



         

            [HttpGet("GetDrugPrescriptionInvoices")]
            public async Task<IActionResult> GetAllServiceInvoice()
            {
                var drugPrescriptions = await _drugPrescription.GetDrugPrescriptionInvoices();

                return Ok(new
                {
                    drugPrescriptions,
                    message = "List of invoice fetched"
                });
            }


            [HttpGet("GetDrugPrescriptionsForInvoice/{invoiceId}")]
            public async Task<IActionResult> GetDrugPrescriptionsByInvoice(string invoiceId)
            {
                var drugPrescriptions = await _drugPrescription.GetDrugPrescriptionsByInvoice(invoiceId);
                if (!drugPrescriptions.Any())
                    return BadRequest(new
                    {
                        response = "400",
                        message = "No Drug Prescription Found In Invoice"
                    });

                return Ok(new
                {
                    drugPrescriptions,
                    message = "List of Prescriptions in invoice fetched"
                });
            }


            [HttpGet("GetDrugPrescriptionInvoicesForPatient/{patientId}")]
            public async Task<IActionResult> GetPatientInvoice(string patientId)
            {
                var patientInvoices = await _drugPrescription.GetDrugPrescriptionInvoicesForPatient(patientId);
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

            [HttpGet("GetDrugPrescription/{DrugPrescriptionId}")]
            public async Task<IActionResult> GetDrugPrescription(string DrugPrescriptionId)
            {
                var drugPrescription = await _drugPrescription.GetDrugPrescription(DrugPrescriptionId);
                if (drugPrescription == null)
                {
                    return BadRequest(new { message = "Invalid post attempt" });
                }

                return Ok(new
                {
                    drugPrescription,
                    message = "Drug Prescription"
                });
            }


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
}
