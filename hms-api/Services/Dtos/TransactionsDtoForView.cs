using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Dtos
{
    public class TransactionsDtoForView
    {
        public string Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TrasactionDate { get; set; }
        public string BenefactorAdmissionId { get; set; }
        public Admission BenefactorAdmission { get; set; }
        public string BenefactorAccountId { get; set; }
        public Account BenefactorAccount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BenefactorAccountPreviousBalance { get; set; }
        public string BenefactorId { get; set; }
        public ApplicationUser Benefactor { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        public string DepositorsName { get; set; }
    }
}
