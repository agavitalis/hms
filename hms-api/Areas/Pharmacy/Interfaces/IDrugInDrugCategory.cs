using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugInDrugCategory
    {
        Task<DrugInDrugCategory> GetDrugInDrugCategoryByIdAsync(string Id);
        Task<IEnumerable<DrugInDrugCategory>> GetAllDrugInDrugCategoriesAsync();
        Task<IEnumerable<DrugInDrugCategory>> GetDrugInDrugCategoryByDrugCategoryIdAsync(string DrugCategoryId);
        Task<bool> CreateDrugInDrugCategoryAsync(CreateDrugInDrugCategoryViewModel createDrugInDrugCategoryVM);

    }
}
