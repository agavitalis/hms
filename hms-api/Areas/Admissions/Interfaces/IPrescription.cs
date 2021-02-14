using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IPrescription
    {
        PagedList<PrescriptionsDtoForView> GetAdmissionPrescriptions(string AdmissionId, PaginationParameter paginationParameter);
        Task<bool> UpdatePrescriptions(AdmissionPrescription patientVitals);
    }
}
