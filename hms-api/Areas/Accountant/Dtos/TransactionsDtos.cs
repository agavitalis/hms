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

}
