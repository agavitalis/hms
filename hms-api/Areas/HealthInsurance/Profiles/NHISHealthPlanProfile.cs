using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;

namespace HMS.Areas.HealthInsurance.Profiles
{
    public class NHISHealthPlanProfile : Profile
    {
        public NHISHealthPlanProfile()
        {
            CreateMap<NHISHealthPlan, NHISHealthPlanDtoForCreate>().ReverseMap();
            CreateMap<NHISHealthPlan, NHISHealthPlanDtoForView>().ReverseMap();
        }
    }
}
