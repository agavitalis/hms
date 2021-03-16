using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOProfile : Profile
    {
        public HMOProfile()
        {
            CreateMap<HMO, HMODtoForCreate>().ReverseMap();
            CreateMap<HMO, HMODtoForView>().ReverseMap();
        }
    }
}
