using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Pharmacy
{
    public class CreateDrugSubCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        public string DrugCategoryId { get; set; }
    }
    public class EditDrugSubCategoryViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DrugCategoryId { get; set; }
    }
}
