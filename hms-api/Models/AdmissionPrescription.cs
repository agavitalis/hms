using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionPrescription
    {
        public AdmissionPrescription()
        {
            Id = Guid.NewGuid().ToString();
            DateGenerated = DateTime.Now;
          
        }

        public string Id { get; set; }
        public string Prescription { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}
