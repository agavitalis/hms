using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Models;

namespace HMS.Areas.Doctor.Profiles
{
    public class SurgeryProfile : Profile
    {
        public SurgeryProfile()
        {
            CreateMap<Surgery, SurgeryDtoForView>().ReverseMap();      
            CreateMap<Surgery, SurgeryDtoForCreate>().ReverseMap();      
            CreateMap<Surgery, OperationNoteOneDtoForUpdate>().ReverseMap();      
            CreateMap<Surgery, OperationNoteTwoDtoForUpdate>().ReverseMap();      
            CreateMap<Surgery, OperationProcedureDtoForUpdate>().ReverseMap();      
        }
    }
}
