using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IServices
    {
        Task<IEnumerable<ServiceCategoryDtoForView>> GetAllServiceCategories();
        Task<ServiceCategory> GetServiceCategoryByIdAsync(string Id);
        Task<bool> CreateServiceCategoryAsync(ServiceCategory serviceCategory);
        Task<bool> UpdateServiceCategory(ServiceCategory serviceToEdit);
        Task<bool> DeleteServiceCategory(ServiceCategory serviceCategory);
        Task<IEnumerable<ServiceDtoForView>> GetAllServices();
        Task<Service> GetServiceByIdAsync(string id);
        Task<bool> CreateService(Service serviceDtoForCreate);
        Task<bool> UpdateService(Service serviceToEdit);
        Task<bool> DeleteService(Service service);

    }
}
