using HMS.Areas.Lab.ViewModels;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabTestCategory
    {
        Task<LabTestCategory> GetCategoryByIdAsync(string Id);
        Task<IEnumerable<LabTestCategory>> GetAllCategoriesAsync();
        Task<bool> CreateCategoryAsync(CreateLabTestCategoryViewModel LabTestCategoryVM);
        Task<bool> EditCategoryAsync(EditLabTestCategoryViewModel LabTestCategoryVM);
        Task<bool> DeleteCategoryAsync(string Id);
        Task<IEnumerable<LabTestCategory>> FindByNameAsync(string Name);
        Task<Int64> TotalNumber();
    }
}
