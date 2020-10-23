using HMS.Models.Pharmacy;
using HMS.ViewModels.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Pharmacy
{
    public interface IDrugInDrugSubCategory
    {
        Task<DrugInDrugSubCategory> GetDrugInDrugSubCategoryIdAsync(string Id);
        Task<IEnumerable<DrugInDrugSubCategory>> GetDrugInDrugSubCategoryByDrugSubCategoryIdAsync(string DrugSubCategoryId);
        Task<IEnumerable<DrugInDrugSubCategory>> GetAllDrugInDrugSubCategoriesAsync();
        Task<bool> CreateDrugInDrugSubCategoryAsync(CreateDrugInDrugSubCategoryViewModel createDrugInDrugSubCategoryVM );
    }
}
