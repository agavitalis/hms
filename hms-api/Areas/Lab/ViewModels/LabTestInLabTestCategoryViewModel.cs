using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.ViewModels
{
    public class LabTestInLabTestCategoryViewModel
    {

    }
    public class CreateLabTestInLabTestCategoryViewModel
    {
        public string LabTestCategoryId { get; set; }
        public string LabTestId { get; set; }
    }

    public class EditLabTestInLabTestCategoryViewModel
    {
        public string Id { get; set; }
        public string LabTestCategoryId { get; set; }
        public string LabTestId { get; set; }
    }
}
