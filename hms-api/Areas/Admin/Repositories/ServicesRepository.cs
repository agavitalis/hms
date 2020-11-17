using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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
    public class ServicesRepository : IServices
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ServicesRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
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
                       Amount = _applicationDbContext.Services.Where(s => s.Id == x).FirstOrDefault().Cost,
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

            var idNotInServices = serviceIds.Where(x => _applicationDbContext.Services.Any(y => y.Id == x));

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
                    AmountTotal = services.Sum(x => x.Cost),
                    Description = serviceRequest.Description,
                    PaymentStatus = "NOT PAID",
                    GeneratedBy = serviceRequest.GeneratedBy,
                    PatientId = serviceRequest.PatientId
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
  
        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoices(PaginationParameter paginationParameter)
        {
            var invoices = await _applicationDbContext.ServiceInvoices.Include(a => a.ServiceRequests).Include(p => p.Patient).ToListAsync();

            var serviceInvoiceToReturn = _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

            return PagedList<ServiceInvoiceDtoForView>.Create(serviceInvoiceToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
            
        }

        //without pagination
        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoices()
        {
            var invoices = await _applicationDbContext.ServiceInvoices.Include(a => a.ServiceRequests).Include(p => p.Patient).ToListAsync();

            return  _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

        }

        public async Task<IEnumerable<dynamic>> GetServiceRequestInAnInvoice(string invoiceId)
        {
            var serviceRequest = await _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == invoiceId)
                .Include(s => s.ServiceInvoice).ThenInclude(i => i.Patient)
                .Include(p => p.Service).ThenInclude(s=>s.ServiceCategory)
                .Select(p => new
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentStatus = p.PaymentStatus,
                    status = p.Status,
                    serviceInvoiceId = p.ServiceInvoiceId,
                    ServiceName = p.Service.Name,
                    serviceCategoryName = p.Service.ServiceCategory.Name,
                    patientFirstName = p.ServiceInvoice.Patient.FirstName,
                    patientLastName = p.ServiceInvoice.Patient.LastName,

                })

                .ToListAsync();

            return serviceRequest;
        }

        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoiceForPatient(string patientId, PaginationParameter paginationParameter)
        {
            var patientProfile = await _applicationDbContext.PatientProfiles.Where(a => a.PatientId == patientId).FirstOrDefaultAsync();

            var invoices = await _applicationDbContext.ServiceInvoices.Where(a => a.PatientId == patientId).Include(p => p.ServiceRequests).ToListAsync();

            var serviceToReturn = _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

            return PagedList<ServiceInvoiceDtoForView>.Create(serviceToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        //without pagination
        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoiceForPatient(string patientId)
        {
            var patientProfile = await _applicationDbContext.PatientProfiles.Where(a => a.PatientId == patientId).FirstOrDefaultAsync();

            var invoices = await _applicationDbContext.ServiceInvoices.Where(a => a.PatientId == patientId).Include(p => p.ServiceRequests).ToListAsync();

            return _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);


        }


        public async Task<bool> CheckIfServiceRequestIdExist(List<string> serviceRequestId)
        {
            if (serviceRequestId == null)
                return false;

            var idNotInServiceRequests = serviceRequestId.Where(x => _applicationDbContext.ServiceRequests.Any(y => y.Id == x));
            return idNotInServiceRequests.Any();
        }


        public async Task<bool> CheckIfAmountPaidIsCorrect(ServiceRequestPaymentDto services)
        {
            if (services == null)
                return false;

            decimal amountTotal = 0;
            //check if the amount tallies
            services.ServiceRequestId.ForEach( serviceRequestId =>
            {
                var service =  _applicationDbContext.ServiceRequests.Where(i => i.Id == serviceRequestId).FirstOrDefault();
                amountTotal = amountTotal + service.Amount;

            });

            if(amountTotal != services.TotalAmount)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> PayForServices(ServiceRequestPaymentDto services)
        {
            int servicesPaid = 0;
            string serviceInvoiceId = "";
           services.ServiceRequestId.ForEach( serviceRequestId =>
           {
               var ServiceRequest = _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
               ServiceRequest.PaymentStatus = "PAID";
               serviceInvoiceId = ServiceRequest.ServiceInvoiceId;

               servicesPaid++;
           });

            _applicationDbContext.SaveChanges();

            //now check of all the servies in this invoice was paid for
            var serviceCount = await  _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == serviceInvoiceId).CountAsync();

            var ServiceInvoice = await _applicationDbContext.ServiceInvoices.FirstOrDefaultAsync(s => s.Id == serviceInvoiceId);

            if (serviceCount == servicesPaid)
                ServiceInvoice.PaymentStatus = "PAID";
            else
                ServiceInvoice.PaymentStatus = "INCOMPLETE";

            await _applicationDbContext.SaveChangesAsync();

            return true;

        }

        public async Task<ServiceRequestResult> UploadServiceRequestResult(ServiceRequestResult serviceRequestResult)
        {
            try
            {
                _applicationDbContext.ServiceRequestResults.Add(serviceRequestResult);
                await _applicationDbContext.SaveChangesAsync();

                return serviceRequestResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UploadServiceRequestResultImage(ServiceUploadResultDto serviceRequestUploadResultDto, string serviceRequestResultId)
        {
           
            try
            {
                if (serviceRequestUploadResultDto != null)
                {
                    for (int i = 0; i < serviceRequestUploadResultDto.Images.Count; i++)
                    {

                        var rootPath = _webHostEnvironment.ContentRootPath;
                        var folderToSaveIn = "wwwroot/Images/";
                        var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                        var absoluteFilePath = "";

                      //  var hostingPath = _hostingEnvironment.WebRootPath,

                        string extension = Path.GetExtension(serviceRequestUploadResultDto.Images[i].FileName);

                        if (ImageValidator.FileSize(_config, serviceRequestUploadResultDto.Images[i].Length) && ImageValidator.Filetype(extension))
                        {
                            if (serviceRequestUploadResultDto.Images != null)
                            {
                                
                                using (var fileStream = new FileStream(Path.Combine(pathToSave, serviceRequestUploadResultDto.Images[i].FileName), FileMode.Create, FileAccess.Write))
                                {
                                   await serviceRequestUploadResultDto.Images[i].CopyToAsync(fileStream);
                                    absoluteFilePath = fileStream.Name;
                                    
                                }

                                // Upload image(s)

                                var image = new ServiceRequestResultImage()
                                {
                                    Image = Path.GetRelativePath(rootPath, absoluteFilePath),
                                    ImageURL = _hostingEnvironment.WebRootFileProvider.GetFileInfo("Images/"+ serviceRequestUploadResultDto.Images[i].FileName)?.PhysicalPath,
                                    ServiceRequestResultId = serviceRequestResultId
                                };
                                _applicationDbContext.ServiceRequestResultImages.Add(image);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                       
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceRequest> GetServiceRequest(string serviceRequestId) => await _applicationDbContext.ServiceRequests.Where(s => s.Id == serviceRequestId).Include(s => s.Service).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync();

        public async Task<int> GetServiceRequestCount() => await _applicationDbContext.ServiceRequests.Where(s => s.Status == "Awaiting Result").CountAsync();
      
        public async Task<IEnumerable<ServiceRequestResult>> GetServiceRequestResults(string serviceRequestId)
        {

            var serviceRequestResults = await _applicationDbContext.ServiceRequestResults
                .Where(s => s.ServiceRequestId == serviceRequestId).Include(s=>s.ServiceRequestResultImages).Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).ToListAsync();

            return serviceRequestResults;
        }
    }
}
