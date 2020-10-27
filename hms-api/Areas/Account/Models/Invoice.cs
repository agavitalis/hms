using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Account.Models
{
    public class Invoice
    {
        public Invoice()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        [Column(TypeName ="decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public string AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public bool PaymentStatus { get; set; }
        public string Summary { get; set; }
        public string PaymentSource { get; set; }

        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string PatientId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
    }
}
