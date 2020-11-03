using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceRequestDto
    {
    }

    public class ServiceRequestDtoForCreate
    {
        public string PatientId { get; set; }
        public List<string> ServiceId { get; set; }
        //public string Amount { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
    }

    public  class ServiceInvoiceDtoForUpdate
    {
        public string PaymentStatus { get; set; }
        public string ModeOfPayment { get; set; }
    }
}
