using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class ServiceInvoice
    {
        public ServiceInvoice()
        {
            Id = Guid.NewGuid().ToString();
            InvoiceNumber = GenerateInvoiceNumber();
            DateGenerated = DateTime.Now;
        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountToBePaidByPatient { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountToBePaidByHMO { get; set; }
        public string PriceCalculationFormular { get; set; }
        public string PaymentStatus { get; set; }

        public string Description { get; set; }
        public string GeneratedBy { get; set; }

        public string PaymentMethod { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }

        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }

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
