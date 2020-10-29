using HMS.Areas.Patient.ViewModels;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientConsultation
    {
        Task<bool> AddPatientToQueueAsync(AddPatientToQueueViewModel PatientQueue);
        Task <object> GetPatientQueue();
        Task<int> CancelPatientConsultationAsync(string patientQueueId);
        Task<int> CompletePatientConsultationAsync(string patientQueueId);
        Task<int> ExpirePatientConsultationAsync(string patientQueueId);
    }
}
