using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientPrescription
    {
        Task<IEnumerable<dynamic>> GetPatientPrescriptionByAppointment(string appointmentId);
        Task<IEnumerable<dynamic>> AllPatientPrescription(string patientId);
        Task<(bool, string)> GenerateInvoice(string[] drugs, string appointmentid);
        Task<IEnumerable<Invoice>> GetAllLabTestInvoices();
        
    }
}
