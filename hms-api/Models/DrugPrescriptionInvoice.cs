using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class DrugPrescriptionInvoice
    {
        public DrugPrescriptionInvoice()
        {
            Id = Guid.NewGuid().ToString();
            InvoiceNumber = GenerateInvoiceNumber();
            DateGenerated = DateTime.Now;
        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AmountTotal { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
        public string ModeOfPayment { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }
        public string ClerkingId { get; set; }
        public DoctorClerking Clerking { get; set; }
      
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
