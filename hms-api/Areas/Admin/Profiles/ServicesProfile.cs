using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            CreateMap<ServiceCategoryDtoForCreate, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryDtoForCreate>();

            CreateMap<ServiceCategoryDtoForUpdate, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryDtoForUpdate>();

            CreateMap<ServiceCategoryDtoForDelete, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryDtoForDelete>();

            CreateMap<ServiceCategory, ServiceCategoryDtoForView>();
            

            CreateMap<ServiceDtoForCreate, Service>();
            CreateMap<Service, ServiceDtoForCreate>();
       

            CreateMap<ServiceDtoForUpdate, Service>();
            CreateMap<Service, ServiceDtoForUpdate>();

            CreateMap<ServiceDtoForDelete, Service>();
            CreateMap<Service, ServiceDtoForDelete>();

            CreateMap<ServiceUploadResultDto, ServiceRequestResult>();
            CreateMap<ServiceRequestResult, ServiceUploadResultDto>();

            CreateMap<Service, ServiceDtoForView>();

            CreateMap<ServiceInvoice, ServiceInvoiceDtoForView>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Patient.FirstName+" "+src.Patient.LastName))
                .ForMember(dest => dest.NoofServices, opt => opt.MapFrom(src => src.ServiceRequests.Count()))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.AmountTotal));

            CreateMap<ServiceRequest, ServiceRequestForView>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus));
        }
            

    }
}
