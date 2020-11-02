using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Repositories
{
    public class LabTestCategoryRepository : ILabTestCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LabTestCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<LabTestCategory> GetCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.LabTestCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<LabTestCategory>> GetAllCategoriesAsync()
        {
            return await _applicationDbContext.LabTestCategories.ToListAsync();
        }

        public async Task<bool> EditCategoryAsync(EditLabTestCategoryViewModel categoryVM)
        {
            var category = await GetCategoryByIdAsync(categoryVM.Id);
            if (category != null)
            {
                category.Name = categoryVM.Name;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateCategoryAsync(CreateLabTestCategoryViewModel categoryVM)
        {
            if (categoryVM != null)
            {
                var newCategory = new LabTestCategory()
                {

                    Name = categoryVM.Name

                };
                _applicationDbContext.LabTestCategories.Add(newCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> TotalNumber()
        {
            var count = await _applicationDbContext.LabTestCategories.LongCountAsync();
            return count;
        }

        public async Task<bool> DeleteCategoryAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var category = _applicationDbContext.LabTestCategories.SingleOrDefault(s => s.Id == Id);
            if (category != null)
            {
                _applicationDbContext.LabTestCategories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<LabTestCategory>> FindByNameAsync(string Name)
        {
            IList<LabTestCategory> categories = new List<LabTestCategory>();
            categories = await _applicationDbContext.LabTestCategories.Where(s => s.Name == Name).ToListAsync();

            return categories;

        }

    }
}

