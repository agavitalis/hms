using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Dtos
{
    public class DrugBatchDtoForView
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class DrugBatchDtoForCreate
    {
        public string DrugId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class DrugBatchDtoForUpdate
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
   
    public class DrugBatchDtoForDelete
    {
        public string Id { get; set; }
    }
}
