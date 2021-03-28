using HMS.Models;
using System;

namespace HMS.Areas.Admissions.Dtos
{
    public class ServiceMedicationDtoForCreate
    {

        public string ServiceId { get; set; }
        public string AdministrationInstruction { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }
    }

    public class ServiceMedicationDtoForView
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public Service Service { get; set; }
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

    public class ServiceMedicationStatusDtoForUpdate
    {
        public string MedicationId { get; set; }
        public string Status { get; set; }
    }

    public class ServiceMedicationDtoForAdminister
    {
        public string ServiceId { get; set; }
        public string InitiatorId { get; set; }
        public string AdmissionId { get; set; }
    }
}
