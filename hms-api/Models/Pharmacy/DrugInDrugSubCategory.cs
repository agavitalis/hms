using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Pharmacy
{
    public class DrugInDrugSubCategory
    {
        public DrugInDrugSubCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //Bring in DrugSubCategory
        public string DrugSubCategoryId { get; set; }

        [ForeignKey("DrugSubCategoryId")]
        public virtual DrugSubCategory DrugSubCategory { get; set; }

        //Bring in Drug
        public string DrugId { get; set; }

        [ForeignKey("DrugId")]
        public virtual Drug Drug { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
