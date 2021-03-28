using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;

namespace HMS.Areas.HealthInsurance.Profiles
{
    public class NHISHealthPlanDrugProfile : Profile
    {
        public NHISHealthPlanDrugProfile()
        {
            CreateMap<NHISHealthPlanDrug, NHISHealthPlanDrugDtoForCreate>().ReverseMap();
            CreateMap<NHISHealthPlanDrug, NHISHealthPlanDrugDtoForUpdate>().ReverseMap();
            CreateMap<NHISHealthPlanDrug, NHISHealthPlanDrugDtoForView>().ReverseMap();
            CreateMap<NHISHealthPlanDrug, NHISHealthPlanDrugDtoForDelete>().ReverseMap();
        }
    }
}
