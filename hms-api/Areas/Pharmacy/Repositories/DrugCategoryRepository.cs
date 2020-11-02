using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Models;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugCategoryRepository : IDrugCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DrugCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<DrugCategory> GetCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.DrugCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<DrugCategory>> GetAllCategoriesAsync()
        {
            return (IEnumerable<DrugCategory>)await _applicationDbContext.DrugCategories.ToListAsync();
        }

        public async Task<bool> EditCategoryAsync(EditDrugCategoryViewModel categoryVM)
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

        public async Task<bool> CreateCategoryAsync(CreateDrugCategoryViewModel categoryVM)
        {
            if (categoryVM != null)
            {
                var newCategory = new DrugCategory()
                {
                  
                    Name = categoryVM.Name
                  
                };
                _applicationDbContext.DrugCategories.Add(newCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> TotalNumber()
        {
            var count = await _applicationDbContext.DrugCategories.LongCountAsync();
            return count;
        }
       
        public async Task<bool> DeleteCategoryAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var category = _applicationDbContext.DrugCategories.Include(s => s.Drug_DrugCategories).SingleOrDefault(s => s.Id == Id);
            if (category != null)
            {
                _applicationDbContext.DrugCategories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<DrugCategory>> FindByNameAsync(string Name)
        {
            IList<DrugCategory> categories = new List<DrugCategory>();
            categories = await _applicationDbContext.DrugCategories.Where(s => s.Name == Name).ToListAsync();

            return categories;

        }

    }
}
