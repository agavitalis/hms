
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models.Lab
{
    public class LabTestCategory
    {
        public LabTestCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        //A one to many relation with LabTestSubCategories
        public ICollection<LabTestSubCategory> LabTestSubCategories { get; set; }

        //A many to many relation with LabTest and LabTestCategories
        public ICollection<LabTestInLabTestCategory> LabTest_LabTestCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
