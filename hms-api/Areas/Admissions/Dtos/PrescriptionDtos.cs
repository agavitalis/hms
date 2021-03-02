using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class PrescriptionsDtoForView
    {
        public string Id { get; set; }
        public string Prescription { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        
        public DateTime DateGenerated { get; set; }
    }

    public class PrescriptionsDtoForUpdate
    {
        public string Prescription { get; set; }
        public string AdmissionId { get; set; }
        public string DoctorId { get; set; }
    }
}
