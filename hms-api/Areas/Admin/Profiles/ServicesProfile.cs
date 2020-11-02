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

            CreateMap<Service, ServiceDtoForView>();
        }

    }
}
