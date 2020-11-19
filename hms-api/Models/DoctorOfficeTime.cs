using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DoctorOfficeTime
    {
        public DoctorOfficeTime()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string WorkDays { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string DoctorProfileId { get; set; }
        public virtual DoctorProfile DoctorProfile { get; set; }
    }

}
