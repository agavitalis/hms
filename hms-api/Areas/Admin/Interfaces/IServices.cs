using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IServices : IGenericRepository<Service>
    {
        Task<IEnumerable<ServiceDtoForView>> GetAllService();
        Task<bool> AddService(ServiceDtoForCreate serviceDtoForCreate);
        Task<bool> UpdateService(ServiceDtoForView serviceToEdit);
        Task<bool> DeleteService(string Id);
    }
}
