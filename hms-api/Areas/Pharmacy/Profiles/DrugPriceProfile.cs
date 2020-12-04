using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Profiles
{
    public class DrugPriceProfile : Profile
    {
        public DrugPriceProfile()
        {
            CreateMap<DrugPrice, DrugPriceDtoForCreate>();
            CreateMap<DrugPriceDtoForCreate, DrugPrice>();

            CreateMap<DrugPrice, DrugPriceDtoForUpdate>();
            CreateMap<DrugPriceDtoForUpdate, DrugPrice>();

            CreateMap<DrugPrice, DrugPriceDtoForDelete>();
            CreateMap<DrugPriceDtoForDelete, DrugPrice>();
        }
    }
}
