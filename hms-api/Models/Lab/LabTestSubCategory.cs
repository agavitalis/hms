
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models.Lab
{
    public class LabTestSubCategory
    {
        public LabTestSubCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        //assign LabTestSubCategory to a LabTestCategory
        public string LabTestCategoryId { get; set; }
        [ForeignKey("LabTestCategoryId")]
        public virtual LabTestCategory LabTestCategory { get; set; }

        //create a many to many relation
        public ICollection<LabTestInLabTestSubCategory> LabTestInLabTestSubCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
