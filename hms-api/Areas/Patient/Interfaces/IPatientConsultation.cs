using HMS.Areas.Patient.ViewModels;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientConsultation
    {
        Task<bool> AddPatientToADoctorConsultationList(AddPatientToADoctorConsultationListViewModel PatientQueue);
        Task <object> GetAPatientConsultationList(string patientId);
        Task<int> CancelPatientConsultationAsync(string patientQueueId);
        Task<int> CompletePatientConsultationAsync(string patientQueueId);
        Task<int> ExpirePatientConsultationAsync(string patientQueueId);
    }
}
