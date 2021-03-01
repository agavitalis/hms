using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Drug Dispensing")]
    [ApiController]
    public class AdmissionDrugDispensingController : Controller
    {
       
        private readonly IMapper _mapper;
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmissionDrugDispensing _admissionDrugDispensing;
        private readonly IDrugInvoicing _drugInvoicing;
        private readonly IDrug _drug;


        public AdmissionDrugDispensingController(IMapper mapper, IAdmission admission, IAdmissionInvoice admissionInvoice, IAdmissionDrugDispensing admissionDrugDispensing, IDrug drug, IDrugInvoicing drugInvoicing)
        {
            _mapper = mapper;
            _admission = admission;
            _drug = drug;
            _admissionInvoice = admissionInvoice;
            _drugInvoicing = drugInvoicing;
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

    }
}
