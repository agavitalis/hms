using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;

namespace HMS.Areas.Admissions.Profiles
{
    public class WardProfile : Profile
    {
        public WardProfile()
        {
            CreateMap<Ward, WardDtoForCreate>();
            CreateMap<WardDtoForCreate, Ward>();

            CreateMap<Ward, WardDtoForUpdate>();
            CreateMap<WardDtoForUpdate, Ward>();

            CreateMap<Ward, WardDtoForDelete>();
            CreateMap<WardDtoForDelete, Ward>();

            CreateMap<Ward, WardDtoForView>();
            
            CreateMap<Bed, BedDtoForView>().ReverseMap();
            CreateMap<Bed, BedDtoForCreate>().ReverseMap();
        }
    }
}
