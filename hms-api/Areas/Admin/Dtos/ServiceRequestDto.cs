using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceInvioceDtoForView
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Cost { get; set; }
        public  int NoofServices { get; set; }
        public string PaymentStatus { get; set; }

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
        public string ModeOfPayment { get; set; }
        public string ReferenceNumber { get; set; }
        public ServiceRequestDtoForUpdate RequestToUpdate { get; set; }
    }

    public class ServiceRequestForView
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string Cost { get; set; }
        public string Status { get; set; }
    }

    public class ServiceRequestDtoForUpdate
    {
        public string ServiceRequestId { get; set; }
        public bool Status { get; set; }
    }
}
