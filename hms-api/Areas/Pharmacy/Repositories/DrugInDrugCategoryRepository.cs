using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Pharmacy.ViewModels;
using HMS.Database;
using HMS.Models.Pharmacy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugInDrugCategoryRepository : IDrugInDrugCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DrugInDrugCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public async Task<DrugInDrugCategory> GetDrugInDrugCategoryByIdAsync(string Id)
        {
            return await _applicationDbContext.DrugInDrugCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<DrugInDrugCategory>> GetDrugInDrugCategoryByDrugCategoryIdAsync(string DrugCategoryId)
        {
            return await _applicationDbContext.DrugInDrugCategories.Where(s => s.DrugCategoryId == DrugCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<DrugInDrugCategory>> GetAllDrugInDrugCategoriesAsync()
        {
            return await _applicationDbContext.DrugInDrugCategories.ToListAsync();
        }

        public async  Task<bool> CreateDrugInDrugCategoryAsync(CreateDrugInDrugCategoryViewModel createDrugInDrugCategoryVM)
        {
            if (createDrugInDrugCategoryVM != null)
            {
                var newCategory = new DrugInDrugCategory()
                {

                    DrugCategoryId = createDrugInDrugCategoryVM.DrugCategoryId.ToString(),
                DrugId = createDrugInDrugCategoryVM.DrugId.ToString(),

                };
                _applicationDbContext.DrugInDrugCategories.Add(newCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

       
    }
}
