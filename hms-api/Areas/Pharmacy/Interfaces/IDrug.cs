using HMS.Areas.Pharmacy.Models;
using HMS.Areas.Pharmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
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
