using HMS.Models.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestInLabTestSubCategoryViewModel;

namespace HMS.Services.Interfaces.Lab
{
    public interface ILabTestInLabTestSubCategory
    {
        Task<LabTestInLabTestSubCategory> GetLabTestInLabTestSubCategoryIdAsync(string Id);
        Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesBySubCategoryIdAsync(string LabTestSubCategoryId);
        Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesAsync();
        Task<bool> CreateLabTestInLabTestSubCategoryAsync(CreateLabTestInLabTestSubCategoryViewModel createLabTestInLabTestSubCategoryVM);
    }
}
