using System.Collections.Generic;


namespace HMS.Areas.Pharmacy.Dtos
{
    public class DrugCostingDto    {
        public string PatientId { get; set; }
        public List<Drugs> Drugs { get; set; }
     
    }

    public class Drugs
    {
        public Drugs()
        {
            numberOfCartons = 0;
            numberOfContainers = 0;
            numberOfUnits = 0;

        }
        public string drugId { get; set; }
        public int numberOfCartons{ get; set; }
        public int numberOfContainers { get; set; }
        public int numberOfUnits { get; set; }
    }

}

