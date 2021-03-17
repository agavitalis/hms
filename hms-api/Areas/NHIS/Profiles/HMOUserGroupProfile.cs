using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOUserGroupProfile : Profile
    {
        public HMOUserGroupProfile()
        {
            CreateMap<HMOUserGroup, HMOUserGroupDtoForCreate>().ReverseMap();
            CreateMap<HMOUserGroup, HMOUserGroupDtoForView>().ReverseMap();
        }
    }
}
