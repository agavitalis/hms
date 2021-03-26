using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;

namespace HMS.Areas.HealthInsurance.Profiles
{
    public class HMOAdminProfileProfile : Profile
    {
        public HMOAdminProfileProfile()
        {
            CreateMap<HMOAdminProfile, HMOAdminDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.HMOAdmin.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.HMOAdmin.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.HMOAdmin.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.HMOAdmin.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.HMOAdmin.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.HMOAdmin.UserType))
                .ReverseMap();

        }
    }
}
