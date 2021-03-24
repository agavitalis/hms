using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOHealthPlanProfile : Profile
    {
        public HMOHealthPlanProfile()
        {
            CreateMap<HMOHealthPlan, HMOHealthPlanDtoForCreate>().ReverseMap();
            CreateMap<HMOHealthPlan, HMOHealthPlanDtoForUpdate>().ReverseMap();
            CreateMap<HMOHealthPlan, HMOHealthPlanDtoForView>().ReverseMap();
        }
    }
}
