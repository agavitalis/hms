using HMS.Models.Pharmacy;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugSubCategoryRepository : IDrugSubCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DrugSubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }


        public async Task<DrugSubCategory> GetDrugSubCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.DrugSubCategories.Include(category => category.DrugCategory).SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<DrugSubCategory>> GetAllDrugSubCategoryAsync()
        {
            return await _applicationDbContext.DrugSubCategories.Include(category => category.DrugCategory).ToListAsync();
        }

        public async Task<bool> EditDrugSubCategoryAsync(EditDrugSubCategoryViewModel subCategoryVM)
        {
            var subCategory = await GetDrugSubCategoryByIdAsync(subCategoryVM.Id);
            if (subCategory != null)
            {
                subCategory.Name = subCategoryVM.Name;
                subCategory.DrugCategoryId = subCategoryVM.DrugCategoryId;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateDrugSubCategoryAsync(CreateDrugSubCategoryViewModel subCategoryVM)
        {
            if (subCategoryVM != null)
            {
                var newSubCategory = new DrugSubCategory()
                {

                    Name = subCategoryVM.Name,
                    DrugCategoryId = subCategoryVM.DrugCategoryId

                };
                _applicationDbContext.DrugSubCategories.Add(newSubCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> Totalnumber()
        {
            var count = await _applicationDbContext.DrugSubCategories.LongCountAsync();
            return count;
        }
        public async Task<bool> DeleteDrugSubCategoryAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var category = _applicationDbContext.DrugSubCategories.Include(s => s.DrugInDrugSubCategories).Include(s => s.DrugCategory).SingleOrDefault(s => s.Id == Id);
            if (category != null)
            {
                _applicationDbContext.DrugSubCategories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<DrugSubCategory>> FindByNameAsync(string Name)
        {
            IList<DrugSubCategory> categories = new List<DrugSubCategory>();
            categories = await _applicationDbContext.DrugSubCategories.Include(category => category.DrugCategory).Where(s => s.Name == Name).ToListAsync();

            return categories;

        }
    }

}
