using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
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
        PagedList<Drug> GetDrugsPagination(PaginationParameter paginationParameter);
        PagedList<Drug> GetDrugsByDrugType(string drugType, PaginationParameter paginationParameter);
       
        Task<IEnumerable<Drug>> GetExpiredDrugs(DateTime date);
        Task<IEnumerable<Drug>> SearchDrugs(string searchString);
        Task<Drug> GetDrug(string DrugId);
        Task<bool> CreateDrug(Drug Drug);
        Task<bool> UpdateDrug(Drug Drug);
        Task<bool> DeleteDrug(Drug Drug);
        Task<bool> CheckIfDrugsExist(List<string> drugIds);
    }
}
