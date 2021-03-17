using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HMOSubUserGroup
    {
        public HMOSubUserGroup()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string HMOUserGroupId { get; set; }
        public HMOUserGroup HMOUserGroup { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
