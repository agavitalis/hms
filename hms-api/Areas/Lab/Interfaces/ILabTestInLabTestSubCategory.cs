using HMS.Areas.Lab.ViewModels;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabTestInLabTestSubCategory
    {
        Task<LabTestInLabTestSubCategory> GetLabTestInLabTestSubCategoryIdAsync(string Id);
        Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesBySubCategoryIdAsync(string LabTestSubCategoryId);
        Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesAsync();
        Task<bool> CreateLabTestInLabTestSubCategoryAsync(CreateLabTestInLabTestSubCategoryViewModel createLabTestInLabTestSubCategoryVM);
    }
}
