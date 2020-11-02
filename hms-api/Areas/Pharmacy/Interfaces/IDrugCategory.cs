using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugCategory
    {
        Task<DrugCategory> GetCategoryByIdAsync(string Id);
        Task<IEnumerable<DrugCategory>> GetAllCategoriesAsync();
        Task<bool> CreateCategoryAsync(CreateDrugCategoryViewModel drugCategoryVM);
        Task<bool> EditCategoryAsync(EditDrugCategoryViewModel drugCategoryVM);
        Task<bool> DeleteCategoryAsync(string Id);
        Task<IEnumerable<DrugCategory>> FindByNameAsync(string Name);
        Task<Int64> TotalNumber();
    }
}
