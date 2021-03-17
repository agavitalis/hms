using System;

namespace HMS.Models
{
    public class HMOUserGroup
    {
        public HMOUserGroup()
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
