using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Service
    {
        public Service()
        {
            DateCreated = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ServiceCategoryId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
