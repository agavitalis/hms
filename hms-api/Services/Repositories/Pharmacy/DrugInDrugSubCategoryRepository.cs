﻿using HMS.Models.Pharmacy;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Services.Interfaces.Pharmacy;
using HMS.ViewModels.Pharmacy;
using System.Linq;

namespace HMS.Services.Repositories.Pharmacy
{
    public class DrugInDrugSubCategoryRepository : IDrugInDrugSubCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DrugInDrugSubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<DrugInDrugSubCategory> GetDrugInDrugSubCategoryIdAsync(string Id)
        {
            return await _applicationDbContext.DrugInDrugSubCategories.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<DrugInDrugSubCategory>> GetDrugInDrugSubCategoryByDrugSubCategoryIdAsync(string DrugSubCategoryId)
        {
            return await _applicationDbContext.DrugInDrugSubCategories.Where(s => s.DrugSubCategoryId == DrugSubCategoryId).ToListAsync();
        }


        public async Task<IEnumerable<DrugInDrugSubCategory>> GetAllDrugInDrugSubCategoriesAsync()
        {
            return await _applicationDbContext.DrugInDrugSubCategories.ToListAsync();
        }

        public async Task<bool> CreateDrugInDrugSubCategoryAsync(CreateDrugInDrugSubCategoryViewModel createDrugInDrugSubCategoryVM)
        {
            if (createDrugInDrugSubCategoryVM != null)
            {
                var newcategory = new DrugInDrugSubCategory()
                {

                    DrugSubCategoryId = createDrugInDrugSubCategoryVM.DrugSubCategoryId.ToString(),
                    DrugId = createDrugInDrugSubCategoryVM.DrugId.ToString()

                };
                _applicationDbContext.DrugInDrugSubCategories.Add(newcategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
