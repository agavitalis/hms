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
    public class ServiceRepository : GenericRepository<Service>, IServices
    {
        private readonly IMapper _mapper;

        public ServiceRepository(ApplicationDbContext context,IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> AddService(ServiceDtoForCreate serviceDtoForCreate)
        {
            var serviceToCreate = _mapper.Map<Service>(serviceDtoForCreate);
            if(serviceToCreate != null)
            {
                var res = await Insert(serviceToCreate);

                return res;
            }

            return false;
        }

        public Task<bool> DeleteService(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ServiceDtoForView>> GetAllService()
        {
            var services = await Get();           

            var servicesToReturn = _mapper.Map<IEnumerable<ServiceDtoForView>>(services);

            return servicesToReturn;
        }

        public async Task<bool> UpdateService(ServiceDtoForView serviceToEdit)
        {
            if (serviceToEdit == null)
                return false;

            var service = _mapper.Map<Service>(serviceToEdit);

            return true;
        }
    }
}
