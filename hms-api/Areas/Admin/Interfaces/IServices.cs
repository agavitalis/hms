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
<<<<<<< HEAD
        Task<bool> AddServiceCategoryAsync(ServiceCategoryDtoForCreate serviceCategor);
        Task<IEnumerable<ServiceCategoryDtoForView>> GetCategoriesAsync();
        Task<ServiceCategory> GetServiceCategoryAsync(string Id);
        Task<IEnumerable<ServiceDtoForView>> GetAllService();
        Task<bool> AddService(ServiceDtoForCreate serviceDtoForCreate);
        Task<bool> UpdateService(ServiceDtoForView serviceToEdit);
=======
        Task<bool> CreateServiceCategoryAsync(ServiceCategory serviceCategory);
        Task<IEnumerable<ServiceCategoryDtoForView>> GetAllServiceCategories();
        Task<ServiceCategory> GetServiceCategoryByIdAsync(string Id);
        Task<IEnumerable<ServiceDtoForView>> GetAllServices();
        Task<Service> GetServiceByIdAsync(string id);
        Task<bool> CreateService(Service serviceDtoForCreate);
        Task<bool> UpdateService(Service serviceToEdit);
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
        Task<bool> DeleteService(string Id);
    }
}
