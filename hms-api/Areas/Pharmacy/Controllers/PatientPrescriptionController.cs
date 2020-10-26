using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class PatientPrescriptionController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public PatientPrescriptionController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }

        [HttpGet]
        [Route("PatientPrescriptionsPerAppointment")]
        public async Task<IActionResult> PatientPrescriptionsPerAppointment(string appointmentid)
        {
            //check if the appointment id is existing
            var appointment = _applicationDbContext.DoctorAppointments.Where(a => a.Id == appointmentid).FirstOrDefault();
            if (appointment == null)
            {
                return BadRequest(new { message = "Please, select a valid appointment id" });
            }
            {
                var prescription = await _applicationDbContext.PatientDrugPrescritions.
                                  Where(p => p.AppointmentId == appointmentid).Join(_applicationDbContext.Drugs,
                                  prescriptions => prescriptions.DrugId,
                                  drugs => drugs.Id,
                                  (prescriptions, drugs) => new { prescriptions, drugs }
                                  ).ToListAsync();

                if (prescription.Count > 0)
                {
                    return Ok(new { prescription });
                }
                return NotFound(new { message = "No prescription is found for this appointment." });

            }

        }
        [HttpGet]
        [Route("ViewAllPatientPrescriptions")]
        public async Task<IActionResult> ViewAllPatientPrescriptions(string patientid)
        {
            //check if patient id is valid
            if (_applicationDbContext.PatientProfiles.Where(p => p.PatientId == patientid).FirstOrDefault() == null)
            {
                return BadRequest(new { message = "Invalid patient id" });
            }

            var allprescriptions = await _applicationDbContext.PatientDrugPrescritions
                                   .Where(p => p.PatientId == patientid).Join(
                                    _applicationDbContext.Drugs,
                                    prescriptions => prescriptions.DrugId,
                                    drugs => drugs.Id,
                                    (prescriptions, drugs) => new { prescriptions, drugs }
                                    ).ToListAsync();

            if (allprescriptions.Count > 0)
            {
                return Ok(new { allprescriptions });
            }
            return NotFound(new { message = "Not prescriptions yet for this patient." });
        }

        [HttpPost]
        [Route("GenerateDrugPrescriptionInvoice")]
        public async Task<IActionResult> GenerateDrugPrescriptionInvoice(string[] drugs, string appointmentid)
        {
            if (drugs == null || appointmentid == null)
            {
                return BadRequest(new { message = "Verify that drugs were selected and an appointment id was sent" });
            }

            //Is appointment id valid

            if (_applicationDbContext.DoctorAppointments.Where(a => a.Id == appointmentid).Count() <= 0)
            {
                return NotFound(new { message = "Appointment id is not found" });
            }
            foreach (var drugid in drugs)
            {
                _applicationDbContext.PatientDrugPrescritions.Where(d => d.DrugId == drugid && d.AppointmentId == appointmentid).FirstOrDefault().isDrugSelected = true;
            }

            Invoice prescriptioninvoice = new Invoice()
            {
                AppointmentId = appointmentid,
                Date = DateTime.Now,
                PaymentStatus = false,
                TotalAmount = SumDrugAmount(drugs),
                Summary = SummarizeDrugs(drugs),
                PaymentSource = "Pharmacy"
               
            };


            _applicationDbContext.Invoices.Add(prescriptioninvoice);
            await _applicationDbContext.SaveChangesAsync();
            return Ok(new { message = "Prescription invoice generated successfully" });
        }

        [HttpGet]
        [Route("ViewAllDrugInvoicesGenerated")]
        public async Task<IActionResult> ViewAllDrugInvoicesGenerated()
        {
            var AllInvoices = await _applicationDbContext.Invoices.ToListAsync();
            if (AllInvoices.Count > 0)
            {
                return Ok(new { AllInvoices });
            }
            return NotFound(new { message = "No Invoice found." });
        }

        private decimal SumDrugAmount(string[] drugs)
        {
            decimal amount = 0;
            foreach (var drugid in drugs)
            {
                amount = amount + _applicationDbContext.Drugs.Where(d => d.Id == drugid).FirstOrDefault().Price;
            }
            return amount;
        }

        private string SummarizeDrugs(string[] drugs)
        {
            string Druglist = "";
            foreach (var drugid in drugs)
            {
                var amount = _applicationDbContext.Drugs.Where(d => d.Id == drugid).FirstOrDefault().Price;
                var name = _applicationDbContext.Drugs.Where(d => d.Id == drugid).FirstOrDefault().Name;
                Druglist = String.Format(Druglist + "{0} - {1}" + ",", name, amount);
            }
            return Druglist;
        }
    }
}
