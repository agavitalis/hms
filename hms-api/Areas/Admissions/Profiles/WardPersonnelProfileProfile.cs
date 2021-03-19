using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;

namespace HMS.Areas.Admissions.Profiles
{
    public class WardPersonnelProfileProfile : Profile
    {
        public WardPersonnelProfileProfile()
        {
            CreateMap<WardPersonnelProfile, WardPersonnelDtoForView>().ReverseMap();
        }
    }
}
