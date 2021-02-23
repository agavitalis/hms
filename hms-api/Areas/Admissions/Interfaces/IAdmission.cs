using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmission
    {
        Task<Admission> GetAdmission(string AdmissionId);
        Task<bool> CreateAdmission(Admission admission);
        Task<bool> UpdateAdmission(Admission admission);
        PagedList<AdmissionDtoForView> GetAdmissionsWithBed(PaginationParameter paginationParameter);
        PagedList<AdmissionDtoForView> GetAdmissionsWithBed(PaginationParameter paginationParameter, string WardId);
        PagedList<AdmissionDtoForView> GetAdmissionsWithoutBed(PaginationParameter paginationParameter);
    }
}
