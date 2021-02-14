using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class Admission
    {
        public Admission()
        {
            Id = Guid.NewGuid().ToString();
            DateOfAdmission = DateTime.Now;
            IsDischarged = false;
        }

        public string Id { get; set; }
        public string AdmissionNote { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string DischargeNote { get; set; }
        public DateTime DateOfDischarge { get; set; }
        /*------ relationships-------*/
        public bool IsDischarged { get; set; }

        [ForeignKey("ApplicationUser")]
        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        public string BedId { get; set; }
        public Bed Bed { get; set; }
        public string AppointmentId { get; set; }
        public string ConsultationId { get; set; }

        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
    }
}
