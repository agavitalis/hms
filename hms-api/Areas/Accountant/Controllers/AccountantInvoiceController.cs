﻿
using System.Threading.Tasks;
using HMS.Areas.Accountant.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant")]
    [ApiController]
    public class AccountantInvoiceController : Controller
    {
        private readonly IAccountantInvoice _feeInvoice;
        public AccountantInvoiceController(IAccountantInvoice feeInvoice)
        {
            _feeInvoice = feeInvoice;
        }

        [Route("GetUnpaidLabInvoices")]
        [HttpGet]
        public async Task<IActionResult> GetUnpaidLabInvoicesAsync()
        {
            
            var LabInvoices = await _feeInvoice.GetUnpaidFeeInvoiceGeneratedByLabAsync();
            if (LabInvoices != null)
            {
                return Ok(new
                {
                    LabInvoices
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "There are no unpaid lab Invoices"
                });
            }
   
        }

        [Route("GetUnpaidLabInvoicesForPatient")]
        [HttpGet]
        public async Task<IActionResult> GetUnpaidLabInvoicesForPatientAsync(string PatientId)
        {
            
                var LabInvoices = await _feeInvoice.GetUnpaidFeeInvoiceGeneratedByLabForPatientAsync(PatientId);
                if (LabInvoices != null)
                {
                    return Ok(new
                    {
                        LabInvoices
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 400,
                        message = "There are no unpaid lab Invoices for this patient"
                    });
                }
          
        }

        [Route("GetUnpaidPharmacyInvoices")]
        [HttpGet]
        public async Task<IActionResult> GetUnpaidPharmacyInvoicesAsync()
        {
           
                var PharmacyInvoices = await _feeInvoice.GetUnpaidFeeInvoiceGeneratedByPharmacyAsync();
                if (PharmacyInvoices != null)
                {
                    return Ok(new
                    {
                        PharmacyInvoices
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 400,
                        message = "There are no unpaid Pharmacy Invoices"
                    });
                }
           
        }

        [Route("GetUnpaidPharmacyInvoicesForPatient")]
        [HttpGet]
        public async Task<IActionResult> GetUnpaidPharmacyInvoicesForPatientAsync(string PatientId)
        {
            
            var PharmacyInvoices = await _feeInvoice.GetUnpaidFeeInvoiceGeneratedByPharmacyForPatientAsync(PatientId);
            if (PharmacyInvoices != null)
            {
                return Ok(new
                {
                    PharmacyInvoices
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "There are no unpaid Pharmacy Invoices"
                });
            }
           
        }
    }
}