using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HMOSubUserGroupPatient
    {
        public HMOSubUserGroupPatient()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public string HMOSubUserGroupId { get; set; }
        public HMOSubUserGroup HMOSubUserGroup { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
