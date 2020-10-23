using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Lab
{
    public class LabTestCategoryViewModel
    {
        public class EditLabTestCategoryViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class CreateLabTestCategoryViewModel
        {
            public string Name { get; set; }
        }
    }
}
