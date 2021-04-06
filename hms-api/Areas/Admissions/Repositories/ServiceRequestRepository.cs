using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
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

namespace HMS.Areas.Admissions.Repositories
{
    public class ServiceRequestRepository : IAdmissionServiceRequest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ServiceRequestRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
    
        }

        public async Task<AdmissionServiceRequest> GetServiceRequest(string serviceRequestId) => await _applicationDbContext.AdmissionServiceRequests.Where(s => s.Id == serviceRequestId).Include(s => s.AdmissionInvoice).Include(s => s.Service).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync();


       

        

        
       
        public PagedList<AdmissionServiceRequestDtoForView> GetAdmissionServiceRequests(string InvoiceId, PaginationParameter paginationParameter)
        {
            var serviceRequests = _applicationDbContext.AdmissionServiceRequests.Include(a => a.AdmissionInvoice.Admission.Patient).Include(a => a.Service).ThenInclude(s => s.ServiceCategory).Where(a => a.AdmissionInvoiceId == InvoiceId).ToList();

            var serviceRequestToReturn = _mapper.Map<IEnumerable<AdmissionServiceRequestDtoForView>>(serviceRequests);

            return PagedList<AdmissionServiceRequestDtoForView>.ToPagedList(serviceRequestToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<AdmissionServiceRequestResult> UploadServiceRequestResult(AdmissionServiceRequestResult serviceRequestResult)
        {
            try
            {
                _applicationDbContext.AdmissionServiceRequestResults.Add(serviceRequestResult);
                await _applicationDbContext.SaveChangesAsync();

                return serviceRequestResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UploadServiceRequestResultImage(AdmissionServiceUploadResultDto serviceRequestResultImage, string serviceRequestResultId)
        {
            try
            {
                if (serviceRequestResultImage != null)
                {
                    for (int i = 0; i < serviceRequestResultImage.Images.Count; i++)
                    {

                        var rootPath = _webHostEnvironment.ContentRootPath;
                        var folderToSaveIn = "wwwroot/Images/";
                        var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                        var absoluteFilePath = "";

                        //  var hostingPath = _hostingEnvironment.WebRootPath,

                        string extension = Path.GetExtension(serviceRequestResultImage.Images[i].FileName);

                        if (ImageValidator.FileSize(_config, serviceRequestResultImage.Images[i].Length) && ImageValidator.Filetype(extension))
                        {
                            if (serviceRequestResultImage.Images != null)
                            {

                                using (var fileStream = new FileStream(Path.Combine(pathToSave, serviceRequestResultImage.Images[i].FileName), FileMode.Create, FileAccess.Write))
                                {
                                    await serviceRequestResultImage.Images[i].CopyToAsync(fileStream);
                                    absoluteFilePath = fileStream.Name;

                                }

                                // Upload image(s)

                                var image = new AdmissionServiceRequestResultImage()
                                {
                                    Image = Path.GetRelativePath(rootPath, absoluteFilePath),
                                    ImageURL = _webHostEnvironment.WebRootFileProvider.GetFileInfo("Images/" + serviceRequestResultImage.Images[i].FileName)?.PhysicalPath,
                                    ServiceRequestResultId = serviceRequestResultId
                                };
                                _applicationDbContext.AdmissionServiceRequestResultImages.Add(image);
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

        public PagedList<AdmissionServiceRequestResultDtoForView> GetServiceRequestResultsPagination(string serviceRequestId, PaginationParameter paginationParameter)
        {
            var serviceRequestResults = _applicationDbContext.AdmissionServiceRequestResults
                .Where(s => s.ServiceRequestId == serviceRequestId).Include(s => s.ServiceRequestResultImages)
                .Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).ToList();

            var serviceRequestResultsToReturn = _mapper.Map<IEnumerable<AdmissionServiceRequestResultDtoForView>>(serviceRequestResults);
            return PagedList<AdmissionServiceRequestResultDtoForView>.ToPagedList(serviceRequestResultsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);

        }


        
    }
}
