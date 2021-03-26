using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;


namespace HMS.Areas.Admin.Profiles
{
    public class AdminProfileProfile : Profile
    {
        public AdminProfileProfile()
        {
            CreateMap<AdminProfile, AdminProfileDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Admin.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Admin.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Admin.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Admin.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Admin.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Admin.UserType))
                .ReverseMap();
        }
    }
}
