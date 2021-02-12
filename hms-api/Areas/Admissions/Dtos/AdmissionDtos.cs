using HMS.Models;
using System;


namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionDtoForView
    {
        public string Id { get; set; }
        public string AdmissionNote { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string DischargeNote { get; set; }
        public DateTime DateOfDischarge { get; set; }
   
        public bool IsDischarged { get; set; }


        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public string BedId { get; set; }
        public Bed Bed { get; set; }
        public string AppointmentId { get; set; }
        public string ConsultationId { get; set; }

        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
    }

    public class AdmissionDtoForBedAssignment
    {
        public string AdmissionId { get; set; }
        public string BedId { get; set; } 
    }
}
