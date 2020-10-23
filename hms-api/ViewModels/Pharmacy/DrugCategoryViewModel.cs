using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Pharmacy
{
    public class EditDrugCategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateDrugCategoryViewModel
    {
        public string Name { get; set; }
    }
}
