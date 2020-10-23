using HMS.Database;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestViewModel;

namespace HMS.Services.Repositories.Lab
{
    public class LabTestRepository : ILabTest
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LabTestRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<LabTest> GetLabTestByIdAsync(string Id)
        {
            return await _applicationDbContext.LabTests.SingleOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<LabTest>> GetAllLabTestsAsync()
        {
            return await _applicationDbContext.LabTests.ToListAsync();
        }

        public async Task<bool> EditLabTestAsync(EditLabTestViewModel editLabTestVM)
        {
            var test = await GetLabTestByIdAsync(editLabTestVM.Id);
            if (test != null)
            {
                test.Name = editLabTestVM.Name;
                test.Price = editLabTestVM.Price;
                test.Description = editLabTestVM.Description;
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateLabTestAsync(CreateLabTestViewModel LabTestVM)
        {
            if (LabTestVM != null)
            {
                var newtest = new LabTest()
                {

                    Name = LabTestVM.Name,
                    Price = LabTestVM.Price,
                    Description = LabTestVM.Description,
                };
                _applicationDbContext.LabTests.Add(newtest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Int64> TotalNummber()
        {
            var count = await _applicationDbContext.LabTests.LongCountAsync();
            return count;
        }
        public async Task<bool> DeleteLabTestAsync(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            var test = _applicationDbContext.LabTests.SingleOrDefault(s => s.Id == Id);
            if (test != null)
            {
                _applicationDbContext.LabTests.Remove(test);
                await _applicationDbContext.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<IEnumerable<LabTest>> FindByNameAsync(string name)
        {
            IList<LabTest> tests = new List<LabTest>();
            tests = await _applicationDbContext.LabTests.Where(s => s.Name == name).ToListAsync();

            return tests;

        }

        public async Task<Int64> TotalNumber()
        {
            var count = await _applicationDbContext.LabTests.LongCountAsync();
            return count;
        }
    }
}
