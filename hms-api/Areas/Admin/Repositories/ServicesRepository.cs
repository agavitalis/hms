using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
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
            var services = await _applicationDbContext.Services.Where(e=>e.ServiceCategoryId == serviceCategoryId).ToListAsync();

            return _mapper.Map<IEnumerable<ServiceDtoForView>>(services);
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

        public async Task<bool> DeleteService(Service service)
        {
            try
            {
                if (service == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Remove(service);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateServiceRequest(ServiceRequestDtoForCreate serviceRequest, string invoiceId)
        {
            try
            {
                if (serviceRequest == null || string.IsNullOrEmpty(invoiceId))
                    return false;

                serviceRequest.ServiceId.ForEach(x => 
                   _applicationDbContext.ServiceRequests.AddAsync(
                   new ServiceRequest
                   {
                       ServiceId = x,
                       Amount = _applicationDbContext.Services.Where(s => s.Id == x).FirstOrDefault().Cost.ToString(),
                       PaymentStatus = "False",
                       ServiceInvoiceId = invoiceId
                   })
                );
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfServicesExist(List<string> serviceIds)
        {
            if (serviceIds == null)
                return false;

            var idNotInServices = serviceIds.Where(x => _applicationDbContext.Services.Any(y => y.Id != x));
            return idNotInServices.Any();
        }

        public async Task<string> GenerateInvoiceForServiceRequest(ServiceRequestDtoForCreate serviceRequest)
        {
            try
            {
                if (serviceRequest == null)
                    return null;

                List<Service> services = new List<Service>();
                foreach (var id in serviceRequest.ServiceId)
                {
                    services.Add(_applicationDbContext.Services.Find(id));
                }

                var serviceInvoice = new ServiceInvoice()
                {
                    AmountTotal = services.Sum(x => x.Cost).ToString(),
                    Description = serviceRequest.Description,
                    PaymentStatus = "False",
                    GeneratedBy = "",
                    PatientProfileId = serviceRequest.PatientId
                };

                await _applicationDbContext.ServiceInvoices.AddAsync(serviceInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return serviceInvoice.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ServiceInvoice>> GetAllServiceRequestInvoices()
        {
            var serviceInvoices = await _applicationDbContext.ServiceInvoices.Include(e=>e.PatientProfile).ToListAsync();
            return serviceInvoices;

        }
  
    
    }
}
