using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrug
    {
        Task<int> GetDrugCount();
        Task<IEnumerable<Drug>> GetDrugs();
        Task<IEnumerable<Drug>> SearchDrugs(string searchString);
        Task<Drug> GetDrug(string DrugId);
        Task<bool> CreateDrug(Drug Drug);
        Task<bool> UpdateDrug(Drug Drug);
        Task<bool> DeleteDrug(Drug Drug);
    }
}
