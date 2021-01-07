using HMS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceInvoiceDtoForView
    {
        public string Id { get; set; }
        public string Fullname { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public  int NoofServices { get; set; }
        public string PaymentStatus { get; set; }
        public string PatientId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime DateGenerated { get; set; }

    }

    public class ServiceRequestDtoForCreate
    {
        public string PatientId { get; set; }
        public List<string> ServiceId { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }

        //this Id can either be Appointment or Consultation Id
        public string Id { get; set; }
        public string IdType { get; set; }
    }

    public class ServiceRequestForView
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public Service Service { get; set; }
        public ServiceInvoice ServiceInvoice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AppointmentId { get; set; }
        public string ConsultationId { get; set; }
        public string IdType { get; set; }
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
        public string InitiatorId { get; set; }
       
    }

    public class ServiceUploadResultDto
    {
        public string ServiceRequestId { get; set; }
        public string Result { get; set; }
        public List<IFormFile> Images { get; set; }
        public string AdditionalComments { get; set; }
    }
}
