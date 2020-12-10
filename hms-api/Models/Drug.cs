using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Drug
    {
        public Drug()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string Measurment { get; set; }
        public string ExpiryDate { get; set; }
        public string DrugType { get; set; }
        public int QuantityInStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public int CostPricePerContainer { get; set; }
        public int QuantityPerContainer { get; set; }
        public int ContainersPerCarton { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultPricePerUnit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultPricePerContainer { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultPricePerCarton { get; set; }

        
        public IList<DrugPrice> DrugPrices { get; set; }
    }
}
