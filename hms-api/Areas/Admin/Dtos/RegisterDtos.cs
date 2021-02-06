using HMS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Areas.Admin.Dtos
{
   
    public class DtoForFileCreation
    {
        public string AccountId { get; set; }
    }

    public class DtoForPatientRegistration
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string HealthPlanId { get; set; }

        //this is optional for personal accounts
        public string AccountId{ get; set; }

        public string InvoiceGeneratedBy { get; set; }
    }

    public class RegistrationInvoiceDtoForView
    {
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }

        public string HealthPlanId { get; set; }
        public virtual HealthPlan HealthPlan { get; set; }

        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string AuthenticationToken { get; set; }
    }
}
