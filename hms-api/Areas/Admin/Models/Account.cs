using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Models
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            IsActive = true;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public int HealthPlanId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public virtual HealthPlan HealthPlan  { get; set; }
    }
}
