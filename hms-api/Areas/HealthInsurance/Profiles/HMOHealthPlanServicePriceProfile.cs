using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;


namespace HMS.Areas.NHIS.Profiles
{
    public class HMOHealthPlanServicePriceProfile : Profile
    {
        public HMOHealthPlanServicePriceProfile()
        {
            CreateMap<HMOHealthPlanServicePrice, HMOHealthPlanServicePriceDtoForCreate>().ReverseMap();
            CreateMap<HMOHealthPlanServicePrice, HMOHealthPlanServicePriceDtoForUpdate>().ReverseMap();
            CreateMap<HMOHealthPlanServicePrice, HMOHealthPlanServicePriceDtoForDelete>().ReverseMap();
        }
    }
}
