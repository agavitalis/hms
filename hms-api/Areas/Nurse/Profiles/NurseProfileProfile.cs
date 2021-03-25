using AutoMapper;
using HMS.Areas.Nurse.Dtos;
using HMS.Models;

namespace HMS.Areas.Nurse.Profiles
{
    public class NurseProfileProfile : Profile
    {
        public NurseProfileProfile()
        {
            //CreateMap<NurseProfile, NurseDtoForView>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Nurse.FirstName));
            CreateMap<NurseProfile, NurseDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Nurse.FirstName)).ReverseMap();
        }   
    }
}
