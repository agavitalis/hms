using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class WardProfile : Profile
    {
        public WardProfile()
        {
            CreateMap<Ward, WardDtoForCreate>();
            CreateMap<WardDtoForCreate, Ward>();

            CreateMap<Ward, WardDtoForUpdate>();
            CreateMap<WardDtoForUpdate, Ward>();

            CreateMap<Ward, WardDtoForDelete>();
            CreateMap<WardDtoForDelete, Ward>();

            CreateMap<Ward, WardDtoForView>();
        }
    }
}
