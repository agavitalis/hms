using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Repositories
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
