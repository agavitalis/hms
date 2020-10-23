using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Lab
{
    public class LabTestInLabTestSubCategory
    {
        public LabTestInLabTestSubCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //Bring in LabTestSubCategory
        public string LabTestSubCategoryId { get; set; }

        [ForeignKey("LabTestSubCategoryId")]
        public virtual LabTestSubCategory LabTestSubCategory { get; set; }

        //Bring in LabTest
        public string LabTestId { get; set; }

        [ForeignKey("LabTestId")]
        public virtual LabTest LabTest { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
