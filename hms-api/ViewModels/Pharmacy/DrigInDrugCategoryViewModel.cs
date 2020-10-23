using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Pharmacy
{
    public class CreateDrugInDrugCategoryViewModel
    {
        public string DrugCategoryId { get; set; }
        public string DrugId { get; set; }
    }

    public class EditDrugInDrugCategoryViewModel
    {
        public string Id { get; set; }
        public string DrugCategoryId { get; set; }
        public string DrugId { get; set; }
    }

  
}
