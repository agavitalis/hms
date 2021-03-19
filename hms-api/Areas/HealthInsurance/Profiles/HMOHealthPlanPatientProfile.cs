using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOHealthPlanPatientProfile : Profile
    {
        public HMOHealthPlanPatientProfile()
        {
            CreateMap<HMOHealthPlanPatient,  HMOHealthPlanPatientDtoForCreate>().ReverseMap();
        }
    }
}
