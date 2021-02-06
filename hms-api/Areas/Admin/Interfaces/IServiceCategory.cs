using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IServiceCategory
    {
        Task<IEnumerable<ServiceCategoryDtoForView>> GetAllServiceCategories();
        PagedList<ServiceCategoryDtoForView> GetServiceCategoriesPagnation(PaginationParameter paginationParameter);
        Task<ServiceCategory> GetServiceCategoryByIdAsync(string serviceCategoryId);
        Task<bool> CreateServiceCategoryAsync(ServiceCategory serviceCategory);
        Task<bool> UpdateServiceCategory(ServiceCategory serviceToEdit);
        Task<bool> DeleteServiceCategory(ServiceCategory serviceCategory);
        Task<IEnumerable<ServiceDtoForView>> GetAllServicesInAServiceCategory(string serviceCategoryId);
        PagedList<ServiceDtoForView> GetAllServicesInAServiceCategoryPagnation(string serviceCategoryId, PaginationParameter paginationParameter);
        Task<int> ServiceCategoryCount();

    }
}

