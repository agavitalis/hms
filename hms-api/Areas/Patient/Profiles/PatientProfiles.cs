using AutoMapper;
using HMS.Areas.Patient.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Profiles
{
    public class PatientProfiles : Profile
    {
        public PatientProfiles()
        {
            CreateMap<PatientDtoForCreate, PatientProfile>();
            CreateMap<PatientProfile, PatientDtoForCreate>();

        }
    }
}
