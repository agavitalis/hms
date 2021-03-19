using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;

namespace HMS.Areas.HealthInsurance.Profiles
{
    public class HMOAdminProfileProfile : Profile
    {
        public HMOAdminProfileProfile()
        {
            CreateMap<HMOAdminProfile, HMOAdminDtoForView>().ReverseMap();
        }
    }
}
