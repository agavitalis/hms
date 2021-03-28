using AutoMapper;
using HMS.Areas.Lab.Dtos;
using HMS.Models;

namespace HMS.Areas.Lab.Profiles
{
    public class LabAttendantProfileProfile : Profile
    {
        public LabAttendantProfileProfile()
        {

            CreateMap<LabProfile, LabAttendantDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Lab.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Lab.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Lab.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Lab.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Lab.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Lab.UserType))
                .ReverseMap();
        }
    }
}
