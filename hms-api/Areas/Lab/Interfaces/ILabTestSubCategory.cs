using HMS.Areas.Lab.Models;
using HMS.Areas.Lab.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabTestSubCategory
    {
        Task<LabTestSubCategory> GetLabTestSubCategoryByIdAsync(string Id);
        Task<IEnumerable<LabTestSubCategory>> GetAllLabTestSubCategoryAsync();
        Task<bool> CreateLabTestSubCategoryAsync(CreateLabTestSubCategoryViewModel subCategotyVM);
        Task<bool> EditLabTestSubCategoryAsync(EditLabTestSubCategoryViewModel subCategotyVM);
        Task<bool> DeleteLabTestSubCategoryAsync(string Id);
        Task<IEnumerable<LabTestSubCategory>> FindByNameAsync(string Name);
        Task<Int64> Totalnumber();
    }
}
