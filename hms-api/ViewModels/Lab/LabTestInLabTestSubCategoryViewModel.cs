using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Lab
{
    public class LabTestInLabTestSubCategoryViewModel
    {
        public class CreateLabTestInLabTestSubCategoryViewModel
        {
            public string LabTestSubCategoryId { get; set; }
            public string LabTestId { get; set; }
        }

        public class EditLabTestInLabTestSubCategoryViewModel
        {
            public string Id { get; set; }
            public string LabTestSubCategoryId { get; set; }
            public string LabTestId { get; set; }
        }
    }
}
