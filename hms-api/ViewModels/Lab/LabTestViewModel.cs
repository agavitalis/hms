using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Lab
{
    public class LabTestViewModel
    {
        public class CreateLabTestViewModel
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }

        public class EditLabTestViewModel
        {
            [Required]
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }
    }
}
