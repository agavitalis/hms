using Microsoft.AspNetCore.JsonPatch;
using System;

namespace HMS.Areas.Pharmacy
{
    public class DrugDtoForCreate
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string Measurment { get; set; }
        public string DrugType { get; set; }
        public decimal CostPricePerContainer { get; set; }
        public int QuantityPerContainer { get; set; }
        public int ContainersPerCarton { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class DrugDtoForCreateExistingDrug
    {
        public string DrugId { get; set; }
        public string SKU { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }


    public class DrugDtoForUpdate
{
    public string Id { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public string GenericName { get; set; }
    public string Manufacturer { get; set; }
    public string Measurment { get; set; }
    public decimal CostPricePerContainer { get; set; }
    public int QuantityPerContainer { get; set; }
    public int ContainersPerCarton { get; set; }
    public DateTime ExpiryDate { get; set; }
}


    public class DrugDtoForPatch
    {
        public string DrugId { get; set; }
        public JsonPatchDocument<DrugDtoForUpdate> Drug { get; set; }
    }

    public class DrugDtoForDelete
    {
        public string Id { get; set; }
    }


}
