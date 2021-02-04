using AutoMapper;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Profiles
{
    public class DrugProfile : Profile
    {
        public DrugProfile()
        {
            CreateMap<Drug, DrugDtoForCreateExistingDrug>().ReverseMap();

            CreateMap<Drug, DrugDtoForCreate>();
            CreateMap<DrugDtoForCreate, Drug>();

            CreateMap<Drug, DrugDtoForUpdate>();
            CreateMap<DrugDtoForUpdate, Drug>();

            CreateMap<Drug, DrugDtoForPatch>();
            CreateMap<DrugDtoForPatch, Drug>();

            CreateMap<Drug, DrugDtoForDelete>();
            CreateMap<DrugDtoForDelete, Drug>();

            //CreateMap<Drug, DrugDtoForView>();
        }
    }
}
