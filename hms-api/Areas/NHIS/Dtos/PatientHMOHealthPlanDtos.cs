using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Dtos
{
    public class PatientHMOHealthPlanDtoForCreate
    {
        public string PatientId { get; set; }
        public string HMOHealthPlanId { get; set; }
    }
}
