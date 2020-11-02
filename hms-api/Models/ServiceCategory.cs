using System;

namespace HMS.Models
{
    public class ServiceCategory
    {
        public ServiceCategory()
        {
            DateCreated = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
