using System;


namespace HMS.Models
{
    public class Drug
    {
        public Drug()
        {
            Id = Guid.NewGuid().ToString();
        }

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
}
