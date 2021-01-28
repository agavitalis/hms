using System;
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
        public string PaymentMethod { get; set; }
        public DateTime TrasactionDate { get; set; }
        public string BenefactorAccountId { get; set; }
        public Account BenefactorAccount { get; set; }

        public string BenefactorId { get; set; }
        public ApplicationUser Benefactor { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        public string DepositorsName { get; set; }
    }
}
