using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionNote
    {
        PagedList<AdmissionNoteDtoForView> GetAdmissionNotes(string AdmissionId, PaginationParameter paginationParameter);
        Task<bool> CreateAdmissionNote(AdmissionNote AdmissionNote);
        Task<AdmissionNote> GetAdmissionNote(string AdmissionNoteId);
    }
}
