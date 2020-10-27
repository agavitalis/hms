using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IServiceCategory : IGenericRepository<ServiceCategory>
    {
        Task<bool> AddServiceCategoryAsync(ServiceCategoryDtoForCreate serviceCategor);
        Task<IEnumerable<ServiceCategoryDtoForView>> GetCategoriesAsync();
        Task<ServiceCategory> GetServiceCategoryAsync(string Id);
    }
}
