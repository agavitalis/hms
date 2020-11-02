using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HealthPlan
    {
        public HealthPlan()
        {
            Id = Guid.NewGuid().ToString();
            this.DateCreated = DateTime.Now;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Renewal { get; set; }

        public int NoOfPatients { get; set; }
        public int NoOfAccounts { get; set; }

        public bool InstantBilling { get; set; }
        public DateTime DateCreated { get; set; } 
        public string CreatedBy { get; set; }
    }
}
