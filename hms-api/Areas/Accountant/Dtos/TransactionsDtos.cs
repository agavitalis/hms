using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Dtos
{
    public class TransactionsDtoForView
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class TransactionTypeDtoForView
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TransactionType { get; set; }
    }

    public class PatientHMOInvoiceDtoForView
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientId { get; set; }
        public string HMOId { get; set; }
    }

    public class DrugHMOInvoiceDtoForView
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DrugId { get; set; }
        public string HMOId { get; set; }
    }

    public class ServiceHMOInvoiceDtoForView
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ServiceId { get; set; }
        public string HMOId { get; set; }
    }

}
