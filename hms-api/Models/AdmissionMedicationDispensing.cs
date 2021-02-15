using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionMedicationDispensing
    {
        public AdmissionMedicationDispensing()
        {
            Id = Guid.NewGuid().ToString();
            DateDispensed = DateTime.Now;
        }
        public string Id { get; set; }
        public string Medication { get; set; }
        public DateTime DateDispensed { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }

    }
}
