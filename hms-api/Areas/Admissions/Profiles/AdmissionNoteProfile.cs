using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class AdmissionNoteProfile : Profile
    {
        public AdmissionNoteProfile()
        {
            CreateMap<AdmissionNote, AdmissionNoteDtoForView>().ReverseMap();
            CreateMap<AdmissionNote, AdmissionNoteDtoForCreate>().ReverseMap();
        }
    }
}
