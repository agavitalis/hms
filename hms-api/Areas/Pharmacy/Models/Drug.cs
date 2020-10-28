
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Models
{
    public class Drug
    {
        public Drug()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public ICollection<DrugInDrugCategory> DrugInDrugCategories { get; set; }
        public ICollection<DrugInDrugSubCategory> DrugInDrugSubCategories { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }
    }
}
