using HMS.Models.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestInLabTestCategoryViewModel;

namespace HMS.Services.Interfaces.Lab
{
    public interface ILabTestInLabTestCategory
    {
        Task<LabTestInLabTestCategory> GetLabTestInLabTestCategoryByIdAsync(string Id);
        Task<IEnumerable<LabTestInLabTestCategory>> GetLabTestInLabTestCategoryByLabTestCategoryIdAsync(string LabTestCategoryId);
        Task<IEnumerable<LabTestInLabTestCategory>> GetAllLabTestInLabTestCategoriesAsync();
        Task<bool> CreateLabTestInLabTestCategoryAsync(CreateLabTestInLabTestCategoryViewModel createLabTestInLabTestCategoryVM);
    }
}
