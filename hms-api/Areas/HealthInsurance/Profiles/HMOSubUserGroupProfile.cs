using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOSubUserGroupProfile : Profile
    {
        public HMOSubUserGroupProfile()
        {
            CreateMap<HMOSubUserGroup, HMOSubUserGroupDtoForCreate>().ReverseMap();
            CreateMap<HMOSubUserGroup, HMOSubUserGroupDtoForUpdate>().ReverseMap();
            CreateMap<HMOSubUserGroup, HMOSubUserGroupDtoForView>().ReverseMap();
        }
    }
}
