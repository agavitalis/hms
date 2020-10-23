using HMS.Models.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Lab.LabTestViewModel;

namespace HMS.Services.Interfaces.Lab
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
