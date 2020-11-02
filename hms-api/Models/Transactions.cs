using System;

namespace HMS.Models
{
    public class Transactions
    {
        public Transactions()
        {
            Id = Guid.NewGuid().ToString();
            TrasactionDate = DateTime.Now;
        }
        public string Id { get; set; }
        public string Amount { get; set; }
        public string TransactionType { get; set; }

        public string InvoiceType { get; set; }
        public string InvoiceTypeId { get; set; }

        public string Description { get; set; }
       
        public DateTime TrasactionDate { get; set; }
    
   
    }
}
