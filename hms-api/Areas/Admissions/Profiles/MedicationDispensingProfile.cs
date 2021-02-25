using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class MedicationDispensingProfile : Profile
    {
        public MedicationDispensingProfile()
        {
            CreateMap<AdmissionMedicationDispensing, AdmissionMedicationDispensingDtoForCreate>().ReverseMap();
            //CreateMap<AdmissionMedicationDispensing, MedicationDispensingDtoForCreate>().ReverseMap();
        }
    }
}
