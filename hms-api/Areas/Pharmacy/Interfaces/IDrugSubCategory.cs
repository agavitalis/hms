using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models.Pharmacy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugSubCategory
    {
        Task<DrugSubCategory> GetDrugSubCategoryByIdAsync(string Id);
        Task<IEnumerable<DrugSubCategory>> GetAllDrugSubCategoryAsync();
        Task<bool> CreateDrugSubCategoryAsync(CreateDrugSubCategoryViewModel subCategotyVM);
        Task<bool> EditDrugSubCategoryAsync(EditDrugSubCategoryViewModel subCategotyVM);
        Task<bool> DeleteDrugSubCategoryAsync(string Id);
        Task<IEnumerable<DrugSubCategory>> FindByNameAsync(string Name);
        Task<Int64> Totalnumber();
    }
}
