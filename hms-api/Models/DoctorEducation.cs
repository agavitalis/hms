using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DoctorEducation
    {
        public DoctorEducation()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string DoctorId { get; set; }
        public virtual DoctorProfile Doctor { get; set; }
    }
}
