using System;

namespace HMS.Models
{
    public class AdmissionServiceMedication
    {
        public AdmissionServiceMedication()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string ServiceId { get; set; }
        public Service Service { get; set; }
        public string AdministrationInstruction { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
    }
}
