using HMS.Models;

namespace HMS.Areas.Admin.Dtos
{
    public class AccountDtoForCreate
    {
        public string Name { get; set; }
        public string HealthPlanId { get; set; }
    }

    public class AccountDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public HealthPlan HealthPlan { get; set; }
    }

    public class AccountDtoForAdminFunding
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string ModeOfPayment { set; get; }
        public string TransactionReference { set; get; }
        public string InvoiceNumber { get; set; }
        public string paymentDescription { set; get; }
    }

    public class AccountDtoForPatientFunding
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string ModeOfPayment { set; get; }
        public string TransactionReference { set; get; }
        public string InvoiceNumber { get; set; }
        public string paymentDescription { set; get; }
    }

    public class AccountDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string  HealthPlanId { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccountDtoForDelete
    {
        public string Id { get; set; }
    }

}
