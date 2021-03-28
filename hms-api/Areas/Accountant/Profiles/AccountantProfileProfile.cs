using AutoMapper;
using HMS.Areas.Accountant.Dtos;
using HMS.Models;

namespace HMS.Areas.Accountant.Profiles
{
    public class AccountantProfileProfile : Profile
    {
        public AccountantProfileProfile()
        {

            CreateMap<AccountantProfile, AccountantDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Accountant.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Accountant.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Accountant.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Accountant.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Accountant.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Accountant.UserType))
                .ReverseMap();
        }
    }
}
