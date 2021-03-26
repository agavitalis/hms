using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;

namespace HMS.Areas.Admissions.Profiles
{
    public class WardPersonnelProfileProfile : Profile
    {
        public WardPersonnelProfileProfile()
        {
            CreateMap<WardPersonnelProfile, WardPersonnelDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.WardPersonnel.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.WardPersonnel.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.WardPersonnel.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.WardPersonnel.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.WardPersonnel.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.WardPersonnel.UserType))
                .ReverseMap();
        }
    }
}
