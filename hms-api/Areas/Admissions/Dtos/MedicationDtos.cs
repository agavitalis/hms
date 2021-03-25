using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;

namespace HMS.Areas.Admissions.Dtos
{
   public class MedicationDtoForView
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

    public class MedicationDtoForCreate
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

    public class MedicationStatusDtoForUpdate
    {
        public string MedicationId { get; set; }
        public string Status { get; set; }
    }

    public class MedicationDtoForAdminister
    {
        public MedicationDtoForAdminister()
        {
            NumberOfCartons = 0;
            NumberOfContainers = 0;
            NumberOfUnits = 0;

        }
        public string DrugId { get; set; }
        public int NumberOfCartons { get; set; }
        public int NumberOfContainers { get; set; }
        public int NumberOfUnits { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }        
    }

}
