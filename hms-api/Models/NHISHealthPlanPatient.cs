using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class NHISHealthPlanPatient
    {
        public NHISHealthPlanPatient()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        public string NHISHealthPlanId { get; set; }
        public virtual NHISHealthPlan NHISHealthPlan { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
