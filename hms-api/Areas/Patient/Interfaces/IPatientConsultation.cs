using HMS.Areas.Patient.ViewModels;
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
        Task <object> GetAPatientConsultations(string patientId);
        Task<int> CancelPatientConsultationAsync(string patientQueueId);
        Task<int> CompletePatientConsultationAsync(string patientQueueId);
        Task<int> ExpirePatientConsultationAsync(string patientQueueId);
    }
}
