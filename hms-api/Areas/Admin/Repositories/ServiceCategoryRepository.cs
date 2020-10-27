using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using HMS.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategory
    {
        private readonly IMapper _mapper;

        public ServiceCategoryRepository(ApplicationDbContext context, IMapper mapper)
            :base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> AddServiceCategoryAsync(ServiceCategoryDtoForCreate serviceCategory)
        {
            if (serviceCategory == null)
                return false;

            var categoryToAdd = _mapper.Map<ServiceCategory>(serviceCategory);

            var res = await Insert(categoryToAdd);
            return res;            
        }

        public async Task<IEnumerable<ServiceCategoryDtoForView>> GetCategoriesAsync()
        {
            var categories = await Get();

            return _mapper.Map<IEnumerable<ServiceCategoryDtoForView>>(categories);
        }

        public async Task<ServiceCategory> GetServiceCategoryAsync(string Id) =>  await GetById(Id);
     
    }
}
