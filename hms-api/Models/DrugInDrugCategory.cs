using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class DrugInDrugCategory
    {
        public DrugInDrugCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //Bring in DrugCategory
        public string DrugCategoryId { get; set; }

        [ForeignKey("DrugCategoryId")]
        public virtual DrugCategory DrugCategory { get; set; }

        //Bring in Drug
        public string DrugId { get; set; }

        [ForeignKey("DrugId")]
        public virtual Drug Drug { get; set; }

        //add default timestamps
        public byte[] RowVersion { get; set; }

    }
}
