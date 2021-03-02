using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionPaymentDto
    {
        public string AdmissionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string InitiatorId { get; set; }
    }

}
