using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;

namespace HMS.Areas.Admin.Profiles
{
    public class HealthPlanProfile :Profile
    {
        public HealthPlanProfile()
        {
            CreateMap<HealthPlan, HealthPlanDtoForCreate>();
            CreateMap<HealthPlanDtoForCreate, HealthPlan>();

            CreateMap<HealthPlan, HealthPlanDtoForView>();
        }
    }
}
