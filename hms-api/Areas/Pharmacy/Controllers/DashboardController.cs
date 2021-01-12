using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy- Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
    
        private readonly IDrug _drug;
        private readonly IDrugInvoicing _drugInvoicing;
        private readonly IDoctorClerking _clerking;


        public DashboardController(IDrug drug, IDrugInvoicing drugInvoicing, IDoctorClerking clerking)
        {
            
            _drug = drug;
            _drugInvoicing = drugInvoicing;
            _clerking = clerking;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
           
            var drugCount = await _drug.GetDrugCount();
            var drugPrescriptionCount = await _clerking.DoctorPrescriptionCount();
            var drugInvoicesPaidNotDispensed = await _drugInvoicing.GetPaidDrugInvoiceNotDispensedCount();
            var drugPaidAndDispensedCount = await await _drugInvoicing.GetPaidDrugInvoiceDispensedCount();

            return Ok(new
            {
                drugCount,
                drugPrescriptionCount,
                drugInvoicesPaidNotDispensed,
                drugPaidAndDispensedCount,
                message = "Pharmacy Dashboard Counts"
            });
        }
    }
}
