using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HMOHealthPlan
    {
        public HMOHealthPlan()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string HMOId { get; set; }
        public HMO HMO { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
