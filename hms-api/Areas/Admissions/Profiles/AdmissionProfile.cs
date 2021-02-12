using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class AdmissionProfile : Profile
    {
        public AdmissionProfile()
        {
            CreateMap<Admission, AdmissionDtoForView>().ReverseMap();
            CreateMap<Admission, AdmissionDtoForBedAssignment>().ReverseMap();
            CreateMap<Admission, AdmissionDtoForDischarge>().ReverseMap();
        }
    }
}
