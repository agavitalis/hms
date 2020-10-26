using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class HealthPlanProfile :Profile
    {
        public HealthPlanProfile()
        {
            CreateMap<HealthPlan, HealthPlanDtoForCreate>();
            CreateMap<HealthPlanDtoForCreate, HealthPlan>();                
        }
    }
}
