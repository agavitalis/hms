using HMS.Areas.Pharmacy.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionRequestDtoForCreate
    {
        public string AdmissionId { get; set; }
        public List<string> ServiceId { get; set; }
        public List<Drugs> Drugs { get; set; }
        public string GeneratedBy { get; set; }
    }
}
