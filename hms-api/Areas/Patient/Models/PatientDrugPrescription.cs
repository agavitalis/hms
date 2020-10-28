using HMS.Areas.Doctor.Models;
using HMS.Areas.Pharmacy.Models;
using HMS.Models;
using System;


namespace HMS.Areas.Patient.Models
{
    public class PatientDrugPrescription
    {
        public PatientDrugPrescription()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Ailment { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string Quantity { get; set; }
        public string Frequency { get; set; }
        public string Dosage { get; set; }
        public string Comment { get; set; }

        public Boolean isDrugSelected { get; set; } = false;
     
        public Boolean isPaid { get; set; } = false;

        public Boolean isDispatched { get; set; } = false;

        /*------ relationships-------*/
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual DoctorAppointment Appointment { get; set; }

    }
}
