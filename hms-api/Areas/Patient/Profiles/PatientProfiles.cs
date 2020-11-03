﻿using AutoMapper;
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
            CreateMap<PatientsDtoForView, PatientProfile>();
            CreateMap<PatientProfile, PatientsDtoForView>();

            CreateMap<PatientProfile, PatientDtoForView>();
            CreateMap<PatientDtoForView, PatientProfile>();
        }
    }
}
