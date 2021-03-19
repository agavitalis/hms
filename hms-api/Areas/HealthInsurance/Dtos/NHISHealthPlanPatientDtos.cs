using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Dtos
{
    public class NHISHealthPlanPatientDtoForCreate
    {
        public string PatientId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanPatientDtoForUpdate
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }
}
