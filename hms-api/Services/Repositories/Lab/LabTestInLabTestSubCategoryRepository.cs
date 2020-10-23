using HMS.Database;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestInLabTestSubCategoryViewModel;

namespace HMS.Services.Repositories.Lab
{
    public class LabTestInLabTestSubCategoryRepository : ILabTestInLabTestSubCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LabTestInLabTestSubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<LabTestInLabTestSubCategory> GetLabTestInLabTestSubCategoryIdAsync(string Id)
        {
            return await _applicationDbContext.LabTestInLabTestSubCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesBySubCategoryIdAsync(string LabTestSubCategoryId)
        {
            return await _applicationDbContext.LabTestInLabTestSubCategories.Where(l => l.LabTestSubCategoryId == LabTestSubCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<LabTestInLabTestSubCategory>> GetAllLabTestInLabTestSubCategoriesAsync()
        {
            return await _applicationDbContext.LabTestInLabTestSubCategories.ToListAsync();
        }

        public async Task<bool> CreateLabTestInLabTestSubCategoryAsync(CreateLabTestInLabTestSubCategoryViewModel createLabTestInLabTestSubCategoryVM)
        {
            if (createLabTestInLabTestSubCategoryVM != null)
            {
                var newcategory = new LabTestInLabTestSubCategory()
                {

                    LabTestSubCategoryId = createLabTestInLabTestSubCategoryVM.LabTestSubCategoryId.ToString(),
                    LabTestId = createLabTestInLabTestSubCategoryVM.LabTestId.ToString()

                };
                _applicationDbContext.LabTestInLabTestSubCategories.Add(newcategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
