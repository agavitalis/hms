using HMS.Models.Pharmacy;
using HMS.ViewModels.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Pharmacy
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
