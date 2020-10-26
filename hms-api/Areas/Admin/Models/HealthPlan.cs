using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Models
{
    public class HealthPlan
    {
        public HealthPlan()
        {
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Renewal { get; set; }
        public int AccountPerPlan { get; set; }
        public bool InstantBilling { get; set; }
        public DateTime DateCreated { get; set; } 
        public string CreatedBy { get; set; }
    }
}
