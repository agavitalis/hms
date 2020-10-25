using HMS.Areas.Lab.ViewModels;
using HMS.Models.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
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
