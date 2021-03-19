using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOSubUserGroupHealthPlanProfile : Profile
    {
        public HMOSubUserGroupHealthPlanProfile()
        {
            CreateMap< HMOSubUserGroupHealthPlan, HMOSubUserGroupHealthPlanDtoForCreate>().ReverseMap();
        }
    }
}
