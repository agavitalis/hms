
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class LabTest
    {
        public LabTest()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public ICollection<LabTestInLabTestCategory> LabCategories { get; set; }
        public ICollection<LabTestSubCategory> LabSubCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
