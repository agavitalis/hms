using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy
{
    public class DrugDtoForCreate
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string DrugType { get; set; }
        public int QuantityInStock { get; set; }
        public int QuantityPerContainer { get; set; }
        public int ContainersPerCarton { get; set; }
    }

    public class DrugDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string DrugType { get; set; }
        public int QuantityInStock { get; set; }
        public int QuantityPerContainer { get; set; }
        public int ContainersPerCarton { get; set; }
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
