using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceInvioceDtoForView
    {
        public string Id { get; set; }
        public string Fullname { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public  int NoofServices { get; set; }
        public string PaymentStatus { get; set; }

    }

    public class ServiceRequestDtoForCreate
    {
        public string PatientId { get; set; }
        public List<string> ServiceId { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
    }

    public class ServiceRequestForView
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }

    public class ServiceRequestPaymentDto
    {
        public string PatientId { get; set; }
        public List<string> ServiceRequestId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public string ModeOfPayment { get; set; }
        public string ReferenceNumber { get; set; }
       
    }


}
