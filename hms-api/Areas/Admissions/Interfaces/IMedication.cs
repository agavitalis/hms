using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IMedication
    {
        PagedList<DrugMedicationDtoForView> GetDrugMedications(string AdmissionId, PaginationParameter paginationParameter);
        Task<AdmissionDrugMedication> GetDrugMedication(string AdmissionMedicationId);
        Task<bool> CreateDrugMedication(AdmissionDrugMedication admission);
        Task<bool> UpdateDrugMedication(AdmissionDrugMedication admission);
        Task<bool> AdministerDrugMedication(AdmissionDrugDispensing admission);
        

        PagedList<ServiceMedicationDtoForView> GetServiceMedications(string AdmissionId, PaginationParameter paginationParameter);
        Task<AdmissionServiceMedication> GetServiceMedication(string AdmissionMedicationId);
        Task<bool> CreateServiceMedication(AdmissionServiceMedication AdmissionRequest);
        Task<bool> UpdateServiceMedication(AdmissionServiceMedication admission);
        Task<bool> AdministerServiceMedication(AdmissionServiceRequest admission);

    }
}
