using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.ViewModels;
using HMS.Database;
using HMS.Models.Lab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HMS.Areas.Lab.Repositories
{
    public class LabTestInLabTestCategoryRepository : ILabTestInLabTestCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LabTestInLabTestCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public async Task<LabTestInLabTestCategory> GetLabTestInLabTestCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.LabTestInLabTestCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<LabTestInLabTestCategory>> GetLabTestInLabTestCategoryByLabTestCategoryIdAsync(string LabTestCategoryId)
        {
            return await _applicationDbContext.LabTestInLabTestCategories.Where(s => s.LabTestCategoryId == LabTestCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<LabTestInLabTestCategory>> GetAllLabTestInLabTestCategoriesAsync()
        {
            return await _applicationDbContext.LabTestInLabTestCategories.ToListAsync();
        }

        public async Task<bool> CreateLabTestInLabTestCategoryAsync(CreateLabTestInLabTestCategoryViewModel createLabTestInLabTestCategoryVM)
        {
            if (createLabTestInLabTestCategoryVM != null)
            {
                var newCategory = new LabTestInLabTestCategory()
                {

                    LabTestCategoryId = createLabTestInLabTestCategoryVM.LabTestCategoryId.ToString(),
                    LabTestId = createLabTestInLabTestCategoryVM.LabTestId.ToString(),

                };
                _applicationDbContext.LabTestInLabTestCategories.Add(newCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
