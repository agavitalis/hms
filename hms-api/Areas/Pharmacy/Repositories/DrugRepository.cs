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
    public class DrugRepository : IDrug
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DrugRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<Drug> GetDrugByIdAsync(string Id)
        {
            return await _applicationDbContext.Drugs.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<Drug>> GetAllDrugsAsync()
        {
            return (IEnumerable<Drug>)await _applicationDbContext.Drugs.ToListAsync();
        }

        public async Task<bool> EditDrugAsync(EditDrugViewModel editDrugVM)
        {
            var test = await GetDrugByIdAsync(editDrugVM.Id);
            if (test != null)
            {
                test.Name = editDrugVM.Name;
                test.Price = editDrugVM.Price;
                test.Description = editDrugVM.Description;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateDrugAsync(CreateDrugViewModel drugVM)
        {
            if (drugVM != null)
            {
                var newtest = new Drug()
                {
                   
                    Name = drugVM.Name,
                    Price = drugVM.Price,
                    Description = drugVM.Description,
                };
                _applicationDbContext.Drugs.Add(newtest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> TotalNummber()
        {
            var count = await _applicationDbContext.Drugs.LongCountAsync();
            return count;
        }
        public async Task<bool> DeleteDrugAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var test = _applicationDbContext.Drugs.Include(d => d.DrugInDrugCategories).Include(d => d.DrugInDrugSubCategories).SingleOrDefault(s => s.Id == Id);
            if (test != null)
            {
                _applicationDbContext.Drugs.Remove(test);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<Drug>> FindByNameAsync(string name)
        {
             IList<Drug> tests = new List<Drug>();
             tests = await _applicationDbContext.Drugs.Where(s => s.Name == name).ToListAsync();
                
             return tests;
            
        }

        public async Task<Int64> TotalNumber()
        {
            var count = await _applicationDbContext.Drugs.LongCountAsync();
            return count;
        }
    }
}
