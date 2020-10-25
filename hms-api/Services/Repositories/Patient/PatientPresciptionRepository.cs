using HMS.Database;
using HMS.Models.Account;
using HMS.Models.Patient;
using HMS.Services.Interfaces.Patient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories.Patient
{
    public class PatientPresciptionRepository : IPatientPrescription
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PatientPresciptionRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<dynamic>> AllPatientPrescription(string patientId)
        {
            
            var allprescriptions = await _applicationDbContext.PatientDrugPrescritions
                                   .Where(p => p.PatientId == patientId)
                                   .Join(_applicationDbContext.Drugs,
                                    prescriptions => prescriptions.DrugId,
                                    drugs => drugs.Id,
                                    (prescriptions, drugs) => new { prescriptions, drugs }
                                    ).ToListAsync();

            return allprescriptions;
        }

        public async Task<(bool, string)> GenerateInvoice(string[] drugs, string appointmentid)
        {
            //Is appointment id valid

            if (_applicationDbContext.DoctorAppointments.Where(a => a.Id == appointmentid).Count() <= 0)
            {
                return (false , "Appointment id is not found");
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

            return (true, "Invoice generated successfully");
        }

        public async Task<IEnumerable<Invoice>> GetAllLabTestInvoices()
        {
            return  await _applicationDbContext.Invoices.ToListAsync();            
        }

        public async Task<IEnumerable<dynamic>> GetPatientPrescriptionByAppointment(string appointmentId)
        {

            var prescription = await _applicationDbContext.PatientDrugPrescritions.
                              Where(p => p.AppointmentId == appointmentId).Join(_applicationDbContext.Drugs,
                              prescriptions => prescriptions.DrugId,
                              drugs => drugs.Id,
                              (prescriptions, drugs) => new { prescriptions, drugs }
                              ).ToListAsync();

            return prescription;

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
