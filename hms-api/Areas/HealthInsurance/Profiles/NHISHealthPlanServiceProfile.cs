using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;

namespace HMS.Areas.HealthInsurance.Profiles
{
    public class NHISHealthPlanServiceProfile : Profile
    {
        public NHISHealthPlanServiceProfile()
        {
            CreateMap<NHISHealthPlanService, NHISHealthPlanServiceDtoForCreate>().ReverseMap();
            CreateMap<NHISHealthPlanService, NHISHealthPlanServiceDtoForUpdate>().ReverseMap();
            CreateMap<NHISHealthPlanService, NHISHealthPlanServiceDtoForDelete>().ReverseMap();
        }
    }
}
