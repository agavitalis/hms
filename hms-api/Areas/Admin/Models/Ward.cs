using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Models
{
    public class Ward
    {
        public Ward()
        {
            DateCreated = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
