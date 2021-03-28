using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugBatch
    {
        Task<DrugBatch> GetDrugBatch(string DrugBatchId);
        Task<DrugBatch> GetDrugBatchByDrug(string DrugId, int quantityOfDrugs);
        PagedList<DrugBatchDtoForView> GetDrugBatchByDrug(string DrugId, PaginationParameter paginationParameter);
        Task<bool> CreateDrugBatch(DrugBatch DrugBatch);
        Task<bool> UpdateDrugBatch(DrugBatch DrugBatch);
        Task<bool> DeleteDrugBatch(DrugBatch DrugBatch);
    }
}
