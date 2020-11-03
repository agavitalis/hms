﻿using System;

namespace HMS.Models
{
    public class ServiceRequest
    {
        public ServiceRequest()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Amount { get; set; }
        public string PaymentStatus { get; set; }

        public string ServiceInvoiceId { get; set; }
        public virtual ServiceInvoice ServiceInvoice {get; set;}
     

        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }

    }
}