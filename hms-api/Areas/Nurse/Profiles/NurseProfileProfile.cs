using AutoMapper;
using HMS.Areas.Nurse.Dtos;
using HMS.Models;

namespace HMS.Areas.Nurse.Profiles
{
    public class NurseProfileProfile : Profile
    {
        public NurseProfileProfile()
        {
            
            CreateMap<NurseProfile, NurseDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Nurse.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Nurse.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Nurse.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Nurse.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Nurse.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Nurse.UserType))
                .ReverseMap();
        }   
    }
}
