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
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.LabAttendant.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LabAttendant.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.LabAttendant.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.LabAttendant.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.LabAttendant.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.LabAttendant.UserType))
                .ReverseMap();
        }
    }
}
