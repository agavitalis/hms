
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Models
{
    public class DrugCategory
    {
        public DrugCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        //A one to many relation with SubCategories
        public ICollection<DrugSubCategory> DrugSubCategories { get; set; }

        //A many to many relation with Drug and Categories
        public ICollection<DrugInDrugCategory> Drug_DrugCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }

    }
}
