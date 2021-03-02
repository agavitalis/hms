using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class AdmissionDrugDispensingProfile : Profile
    {
        public AdmissionDrugDispensingProfile()
        {
            CreateMap<AdmissionDrugDispensing, AdmissionDrugDispensingDtoForCreate>().ReverseMap();
            CreateMap<AdmissionDrugDispensing, DrugDispensingPaymentDto>().ReverseMap();
            CreateMap<AdmissionDrugDispensing, AdmissionDrugDispensingDtoForView>().ReverseMap();
        }

    }
}
