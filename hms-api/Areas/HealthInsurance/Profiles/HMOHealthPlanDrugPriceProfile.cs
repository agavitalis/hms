using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOHealthPlanDrugPriceProfile : Profile
    {
        public HMOHealthPlanDrugPriceProfile()
        {
            CreateMap<HMOHealthPlanDrugPrice, HMOHealthPlanDrugPriceDtoForCreate>().ReverseMap();
            CreateMap<HMOHealthPlanDrugPrice, HMOHealthPlanDrugPriceDtoForUpdate>().ReverseMap();
            CreateMap<HMOHealthPlanDrugPrice, HMOHealthPlanDrugPriceDtoForView>().ReverseMap();
            CreateMap<HMOHealthPlanDrugPrice, HMOHealthPlanDrugPriceDtoForDelete>().ReverseMap();
        }       
    }
}
