﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }
        public DateTime TrasactionDate { get; set; }
    
   
    }
}
