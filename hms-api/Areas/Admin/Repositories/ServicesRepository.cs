using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using HMS.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ServicesRepository : IServices
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
  

        public ServicesRepository(ApplicationDbContext applicationDbContext, IMapper mapper) 
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }


        public async Task<bool> CreateServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            try
            {
                if (serviceCategory == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceCategories.Add(serviceCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public async Task<IEnumerable<ServiceCategoryDtoForView>> GetAllServiceCategories()
        {
            var categories = await _applicationDbContext.ServiceCategories.ToListAsync();

            return _mapper.Map<IEnumerable<ServiceCategoryDtoForView>>(categories);
        }

        public async Task<ServiceCategory> GetServiceCategoryByIdAsync(string id)
        {
            try
            {
                var serviceCategory = await _applicationDbContext.ServiceCategories.FindAsync(id);

                return serviceCategory;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<bool> CreateService(Service service)
        {
            try
            {
                if (service == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Add(service);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Task<bool> DeleteService(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ServiceDtoForView>> GetAllServices()
        {
            var services = await _applicationDbContext.Services.ToListAsync();

            return _mapper.Map<IEnumerable<ServiceDtoForView>>(services);
        }

        public async Task<Service> GetServiceByIdAsync(string id)
        {
            try
            {
                var service = await _applicationDbContext.Services.FindAsync(id);

                return service;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateService(Service serviceToEdit)
        {
            try
            {
                if (serviceToEdit == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Update(serviceToEdit);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
