using HMS.Models.Pharmacy;
using HMS.ViewModels.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Pharmacy
{
    public interface IDrugInDrugCategory
    {
        Task<DrugInDrugCategory> GetDrugInDrugCategoryByIdAsync(string Id);
        Task<IEnumerable<DrugInDrugCategory>> GetAllDrugInDrugCategoriesAsync();
        Task<IEnumerable<DrugInDrugCategory>> GetDrugInDrugCategoryByDrugCategoryIdAsync(string DrugCategoryId);
        Task<bool> CreateDrugInDrugCategoryAsync(CreateDrugInDrugCategoryViewModel createDrugInDrugCategoryVM);

    }
}
