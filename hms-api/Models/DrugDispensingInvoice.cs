using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class DrugDispensingInvoice
    {
        public DrugDispensingInvoice()
        {
            Id = Guid.NewGuid().ToString();
            InvoiceNumber = GenerateInvoiceNumber();
            DateGenerated = DateTime.Now;
        }
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal AmountTotal { get; set; }
        public string PaymentStatus { get; set; }
        
        public string GeneratedBy { get; set; }
        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        public DateTime DateGenerated { get; set; }

        public string ModeOfPayment { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public DateTime DatePaid { get; set; }
        public string PaidBy { get; set; }

        public string ClerkingId { get; set; }
        public DoctorClerking Clerking { get; set; }

        public virtual ICollection<DrugDispensing> DrugDispensing { get; set; }

        public string GenerateInvoiceNumber()
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            string formatedAccountNumber = "HMS" + str_build.ToString();
            return formatedAccountNumber;
        }
    }
}
