﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
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
            CreateMap<ServiceCategory, ServiceCategoryDtoForView>();
            CreateMap<ServiceCategoryDtoForCreate, ServiceCategory>();

            CreateMap<ServiceDtoForCreate, Service>();

            CreateMap<Service, ServiceDtoForView>();
        }
    }
}