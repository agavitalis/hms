using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Dtos
{
    public class DrugInvoicingDto    {
        public string PatientId { get; set; }
        public string GeneratedBy { get; set; }
        public string ClarkingId { get; set; }
        public List<Drugs> Drugs { get; set; }
     
    }

}

