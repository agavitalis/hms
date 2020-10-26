using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.ViewModels
{
    public class LabTestSubCategoryViewModel
    {
      
      
    }

    public class EditLabTestSubCategoryViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LabTestCategoryId { get; set; }
    }

    public class CreateLabTestSubCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        public string LabTestCategoryId { get; set; }
    }
}

