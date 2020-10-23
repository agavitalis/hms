using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Pharmacy
{
    public class DrugSubCategory
    {
        public DrugSubCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
       
        //assign subcategory to a category
        public string DrugCategoryId { get; set; }
        [ForeignKey("DrugCategoryId")]
        public virtual DrugCategory DrugCategory { get; set; }

        //create a many to many relation
        public ICollection<DrugInDrugSubCategory> DrugInDrugSubCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
