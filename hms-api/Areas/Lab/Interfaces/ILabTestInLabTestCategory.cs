using HMS.Areas.Lab.ViewModels;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabTestInLabTestCategory
    {
        Task<LabTestInLabTestCategory> GetLabTestInLabTestCategoryByIdAsync(string Id);
        Task<IEnumerable<LabTestInLabTestCategory>> GetLabTestInLabTestCategoryByLabTestCategoryIdAsync(string LabTestCategoryId);
        Task<IEnumerable<LabTestInLabTestCategory>> GetAllLabTestInLabTestCategoriesAsync();
        Task<bool> CreateLabTestInLabTestCategoryAsync(CreateLabTestInLabTestCategoryViewModel createLabTestInLabTestCategoryVM);
    }
}
