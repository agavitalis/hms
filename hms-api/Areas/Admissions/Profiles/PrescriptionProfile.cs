using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class PrescriptionProfile : Profile
    {
        public PrescriptionProfile()
        {
           

            CreateMap<AdmissionPrescription, PrescriptionsDtoForView>().ReverseMap();
            CreateMap<AdmissionPrescription, PrescriptionsDtoForUpdate>().ReverseMap();
        }
    }
}
