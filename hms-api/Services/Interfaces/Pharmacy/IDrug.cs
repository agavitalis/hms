using HMS.Models.Pharmacy;
using HMS.ViewModels.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Pharmacy
{
    public interface IDrug
    {
        Task<Drug> GetDrugByIdAsync(string drugId);
        Task<IEnumerable<Drug>> GetAllDrugsAsync();
        Task<bool> CreateDrugAsync(CreateDrugViewModel drugVM);
        Task<bool> EditDrugAsync(EditDrugViewModel drugVM);
        Task<bool> DeleteDrugAsync(string drugId);
        Task<IEnumerable<Drug>> FindByNameAsync(string Name);
        Task<Int64> TotalNumber();

    }
}
