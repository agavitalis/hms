using System;

namespace HMS.Models
{
    public class DrugBatch
    {
        public DrugBatch()
        {
            Id = Guid.NewGuid().ToString();
            IsActive = false;
        }

        public string Id { get; set; }
        public string DrugId { get; set; }
        public Drug Drug { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
