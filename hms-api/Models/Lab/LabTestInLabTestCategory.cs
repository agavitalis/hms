using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Lab
{
    public class LabTestInLabTestCategory
    {
        public LabTestInLabTestCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //Bring in LabTestCategory
        public string LabTestCategoryId { get; set; }

        [ForeignKey("LabTestCategoryId")]
        public virtual LabTestCategory LabTestCategory { get; set; }

        //Bring in LabTest
        public string LabTestId { get; set; }

        [ForeignKey("LabTestId")]
        public virtual LabTest LabTest { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }

    }
}
