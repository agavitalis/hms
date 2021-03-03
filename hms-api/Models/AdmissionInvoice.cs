using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class AdmissionInvoice
    {
        public AdmissionInvoice()
        {
            Id = Guid.NewGuid().ToString();
            InvoiceNumber = GenerateInvoiceNumber();
            DateGenerated = DateTime.Now;
        }

        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public DateTime DateGenerated { get; set; }
        public DateTime DatePaid { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }

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
