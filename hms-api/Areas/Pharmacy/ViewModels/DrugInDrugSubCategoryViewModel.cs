using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.ViewModels
{
    public class CreateDrugInDrugSubCategoryViewModel
    {
        public string DrugSubCategoryId { get; set; }
        public string DrugId { get; set; }
    }

    public class EditDrugInDrugSubCategoryViewModel
    {
        public string Id { get; set; }
        public string DrugSubCategoryId { get; set; }
        public string DrugId { get; set; }
    }
}
