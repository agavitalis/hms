using HMS.Areas.Lab.Models;
using HMS.Areas.Lab.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Interfaces
{
    public interface ILabTest
    {
        Task<LabTest> GetLabTestByIdAsync(string LabTestId);
        Task<IEnumerable<LabTest>> GetAllLabTestsAsync();
        Task<bool> CreateLabTestAsync(CreateLabTestViewModel LabTestVM);
        Task<bool> EditLabTestAsync(EditLabTestViewModel LabTestVM);
        Task<bool> DeleteLabTestAsync(string LabTestId);
        Task<IEnumerable<LabTest>> FindByNameAsync(string Name);
        Task<Int64> TotalNumber();
    }
}
