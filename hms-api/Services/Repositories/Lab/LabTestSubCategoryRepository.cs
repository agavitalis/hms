using HMS.Database;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestSubCategoryViewModel;

namespace HMS.Services.Repositories.Lab
{
    public class LabTestSubCategoryRepository : ILabTestSubCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LabTestSubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }


        public async Task<LabTestSubCategory> GetLabTestSubCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.LabTestSubCategories.Include(category => category.LabTestCategory).SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<LabTestSubCategory>> GetAllLabTestSubCategoryAsync()
        {
            return await _applicationDbContext.LabTestSubCategories.Include(category => category.LabTestCategory).ToListAsync();
        }

        public async Task<bool> EditLabTestSubCategoryAsync(EditLabTestSubCategoryViewModel subCategoryVM)
        {
            var subCategory = await GetLabTestSubCategoryByIdAsync(subCategoryVM.Id);
            if (subCategory != null)
            {
                subCategory.Name = subCategoryVM.Name;
                subCategory.LabTestCategoryId = subCategoryVM.LabTestCategoryId;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateLabTestSubCategoryAsync(CreateLabTestSubCategoryViewModel subCategoryVM)
        {
            if (subCategoryVM != null)
            {
                var newSubCategory = new LabTestSubCategory()
                {

                    Name = subCategoryVM.Name,
                    LabTestCategoryId = subCategoryVM.LabTestCategoryId

                };
                _applicationDbContext.LabTestSubCategories.Add(newSubCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> Totalnumber()
        {
            var count = await _applicationDbContext.LabTestSubCategories.LongCountAsync();
            return count;
        }
        public async Task<bool> DeleteLabTestSubCategoryAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var category = _applicationDbContext.LabTestSubCategories.SingleOrDefault(s => s.Id == Id);
            if (category != null)
            {
                _applicationDbContext.LabTestSubCategories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<LabTestSubCategory>> FindByNameAsync(string Name)
        {
            IList<LabTestSubCategory> categories = new List<LabTestSubCategory>();
            categories = await _applicationDbContext.LabTestSubCategories.Include(category => category.LabTestCategory).Where(s => s.Name == Name).ToListAsync();

            return categories;

        }
    }
}
