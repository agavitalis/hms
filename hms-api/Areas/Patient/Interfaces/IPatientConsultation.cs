using HMS.Areas.Admin.Dtos;
using HMS.Areas.Patient.ViewModels;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientConsultation
    {
        Task<int> GetPendingConsultationsCount(string patientId);
        Task<int> GetCompletedConsultationsCount(string patientId);
        Task<int> GetCanceledConsultationsCount(string patientId);
        Task<bool> BookConsultation(BookConsultation patientConsultatione);
        PagedList<ConsultationDtoForView> GetPendingConsultations(string PatientId, PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetCanceledConsultations(string PatientId, PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetCompletedConsultations(string PatientId, PaginationParameter paginationParameter);
        Task<int> CancelPatientConsultationAsync(string patientQueueId);
        Task<int> CompletePatientConsultationAsync(string patientQueueId);
        Task<int> ExpirePatientConsultationAsync(string patientQueueId);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
    }
}
