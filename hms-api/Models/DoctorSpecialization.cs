using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DoctorSpecialization
    {
        public DoctorSpecialization()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Specialization { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string DoctorProfileId { get; set; }
        public virtual DoctorProfile DoctorProfile { get; set; }
    }
}
