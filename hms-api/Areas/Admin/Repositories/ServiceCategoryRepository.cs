using System;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ServiceCategoryRepository : IServiceCategory
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ITransactionLog _transaction;

        public ServiceCategoryRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHostingEnvironment hostingEnvironment, IConfiguration config, ITransactionLog transaction)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _transaction = transaction;
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

        public async Task<bool> UpdateServiceCategory(ServiceCategory serviceToEdit)
        {
            try
            {
                if (serviceToEdit == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceCategories.Update(serviceToEdit);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteServiceCategory(ServiceCategory serviceCategory)
        {
            try
            {
                if (serviceCategory == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceCategories.Remove(serviceCategory);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ServiceDtoForView>> GetAllServicesInAServiceCategory(string serviceCategoryId)
        {
            var services = await _applicationDbContext.Services.Where(e => e.ServiceCategoryId == serviceCategoryId).ToListAsync();

            return _mapper.Map<IEnumerable<ServiceDtoForView>>(services);
        }

        public async Task<int> ServiceCategoryCount()
        {
            var serviceCategoryCount = await _applicationDbContext.ServiceCategories.CountAsync();
            return serviceCategoryCount;
        }

    }
}
