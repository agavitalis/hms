using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugCosting
    {
        Task<bool> CheckIfDrugsExist(List<Drugs> drugs);
        Task<object> CostDrugs(DrugCostingDto drugCosting);
       
    }
}
