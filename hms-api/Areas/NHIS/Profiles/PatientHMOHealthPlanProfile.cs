using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class PatientHMOHealthPlanProfile : Profile
    {
        public PatientHMOHealthPlanProfile()
        {
            CreateMap<PatientHMOHealthPlan,  PatientHMOHealthPlanDtoForCreate>().ReverseMap();
        }
    }
}
