using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionNoteDtoForView
    {
        public string Id { get; set; }
        public string Note { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public DateTime DateGenerated { get; set; }
    }

    public class AdmissionNoteDtoForCreate
    {
        public string Note { get; set; }
        public string AdmissionId { get; set; }
        public string DoctorId { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}
