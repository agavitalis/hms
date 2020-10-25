using HMS.Models.Account;
using HMS.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Patient
{
    public interface IPatientPrescription
    {
        Task<IEnumerable<dynamic>> GetPatientPrescriptionByAppointment(string appointmentId);
        Task<IEnumerable<dynamic>> AllPatientPrescription(string patientId);
        Task<(bool, string)> GenerateInvoice(string[] drugs, string appointmentid);
        Task<IEnumerable<Invoice>> GetAllLabTestInvoices();
        
    }
}
