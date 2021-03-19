using HMS.Areas.Admin.Dtos;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorConsultation
    {
        PagedList<ConsultationDtoForView> GetConsultationsOnOpenList(string DoctorId, PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetConsultationsWithDoctor(string DoctorId, PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetConsultationsCompleted(string DoctorId, PaginationParameter paginationParameter);
    }
}
