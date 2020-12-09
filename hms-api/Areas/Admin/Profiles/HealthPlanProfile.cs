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

            CreateMap<HealthPlan, HealthPlanDtoForUpdate>();
            CreateMap<HealthPlanDtoForUpdate, HealthPlan>();

            CreateMap<HealthPlan, HealthPlanDtoForDelete>();
            CreateMap<HealthPlanDtoForDelete, HealthPlan>();

            CreateMap<HealthPlan, HealthPlanDtoForView>();
        }
    }
}
