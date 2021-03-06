using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
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
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChargePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
