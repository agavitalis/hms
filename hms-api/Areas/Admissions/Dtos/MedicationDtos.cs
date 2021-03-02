using HMS.Models;
using System;

namespace HMS.Areas.Admissions.Dtos
{
   public class MedicationDtoForView
   {
        public string Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        
        
    }

    public class MedicationDtoForCreate
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }
    }

}
