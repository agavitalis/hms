using HMS.Areas.Pharmacy.Models;
using HMS.Areas.Pharmacy.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugInDrugSubCategory
    {
        Task<DrugInDrugSubCategory> GetDrugInDrugSubCategoryIdAsync(string Id);
        Task<IEnumerable<DrugInDrugSubCategory>> GetDrugInDrugSubCategoryByDrugSubCategoryIdAsync(string DrugSubCategoryId);
        Task<IEnumerable<DrugInDrugSubCategory>> GetAllDrugInDrugSubCategoriesAsync();
        Task<bool> CreateDrugInDrugSubCategoryAsync(CreateDrugInDrugSubCategoryViewModel createDrugInDrugSubCategoryVM );
    }
}
