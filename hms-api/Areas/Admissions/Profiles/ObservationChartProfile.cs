using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class ObservationChartProfile : Profile
    {
        public ObservationChartProfile()
        {
            CreateMap<ObservationChart, ObservationChartDtoForUpdate>().ReverseMap();
        }
    }
}
