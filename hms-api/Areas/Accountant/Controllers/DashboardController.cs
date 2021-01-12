using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant", Name = "Accountant - Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
       
        private readonly IRegistrationInvoice _registrationInvoice;
        private readonly IServiceRequestInvoice _serviceRequestInvoice;
        private readonly IDrugInvoicing _drugInvoicing;

        public DashboardController(IRegistrationInvoice registrationInvoice, IServiceRequestInvoice serviceRequestInvoice, IDrugInvoicing drugInvoicing)
        {

            _registrationInvoice = registrationInvoice;
            _serviceRequestInvoice = serviceRequestInvoice;
            _drugInvoicing = drugInvoicing;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {     
            var paidRegistrationInvoice = await _registrationInvoice.GetPaidRegistrationInvoicesCount();
            var unPaidRegistrationInvoice = await _registrationInvoice.GetUnPaidRegistrationInvoicesCount();

            var paidServiceRequestInvoiceCount = await _serviceRequestInvoice.GetPaidServiceRequestInvoiceCount();
            var unPaidServiceRequestInvoiceCount = await _serviceRequestInvoice.GetUnPaidServiceRequestInvoiceCount();

            var paidDrugInvoiceCount = await _drugInvoicing.GetPaidDrugInvoiceCount();
            var unPaidDrugInvoiceCount = await _drugInvoicing.GetUnPaidDrugInvoiceCount();

            return Ok(new
            {
                paidRegistrationInvoice,
                unPaidRegistrationInvoice,
                paidServiceRequestInvoiceCount,
                unPaidServiceRequestInvoiceCount,
                paidDrugInvoiceCount,
                unPaidDrugInvoiceCount,
                message = "Accountant Dashboard Counts"
            });
        }
    }
}
