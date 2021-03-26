using HMS.Models;
using System;

namespace HMS.Areas.Admissions.Dtos
{
   public class DrugMedicationDtoForView
   {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public Drug Drug { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string AdministrationInstruction { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        
        
    }

    public class DrugMedicationDtoForCreate
    {
        public string DrugId { get; set; }
        public string AdministrationInstruction { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }
    }

    public class DrugMedicationStatusDtoForUpdate
    {
        public string MedicationId { get; set; }
        public string Status { get; set; }
    }

    public class DrugMedicationDtoForAdminister
    {
        public string DrugId { get; set; }
        public int NumberOfCartons { get; set; }
        public int NumberOfContainers { get; set; }
        public int NumberOfUnits { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }        
    }

}
