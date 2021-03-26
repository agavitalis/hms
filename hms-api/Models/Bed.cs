using System;

namespace HMS.Models
{
    public class Bed
    {
        public Bed()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            IsAvailable = true;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string WardId { get; set; }
        public Ward Ward { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
