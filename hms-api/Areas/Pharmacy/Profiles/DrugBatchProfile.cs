using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;

namespace HMS.Areas.Pharmacy.Profiles
{
    public class DrugBatchProfile : Profile
    {
        public DrugBatchProfile()
        {
            CreateMap<DrugBatch, DrugBatchDtoForView>().ReverseMap();
            CreateMap<DrugBatch, DrugBatchDtoForCreate>().ReverseMap();
            CreateMap<DrugBatch, DrugBatchDtoForUpdate>().ReverseMap();
            CreateMap<DrugBatch, DrugBatchDtoForDelete>().ReverseMap();
        }
    }
}
