using HMS.Areas.Admin.Dtos;
using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IConsultation
    {
        Task<int> GetConsultationCount();
        Task<int> GetPatientsOnOpenListCount();
        Task<int> GetPatientsUnattendedToCount();
        Task<int> GetPatientsAttendedToCount();
        Task<dynamic> GetConsultations();
        PagedList<ConsultationDtoForView> GetConsultationsPagination(PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetConsultationsOnOpenList(PaginationParameter paginationParameter);
        PagedList<ConsultationDtoForView> GetConsultationsWithDoctors(PaginationParameter paginationParameter);

        PagedList<ConsultationDtoForView> GetConsultationsCompleted(PaginationParameter paginationParameter);
        Task<MyPatient> CheckDoctorInMyPatients(string DoctorId, string PatientId);
        Task<Consultation> GetConsultationById(string Id);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
        Task<bool> BookConsultation(Consultation patientConsultation);
        Task<bool> UpdateConsultation(Consultation consultation);
        Task<bool> DeleteConsultation(Consultation consultation);
        Task<int> CancelPatientConsultationAsync(string consultationId);
        Task<int> AdmitPatientOrSendPatientHome(CompletDoctorClerkingDto clerking);
        Task<int> ExpirePatientConsultationAsync(string consultationId);
        Task<bool> ReassignPatientToNewDoctor(Consultation consultation, JsonPatchDocument<ConsultationDtoForUpdate> Consultation);
        Task<int> GetDoctorsPendingConsultationCount(string doctorId);
        Task<int> GetDoctorsCompletedConsultationCount(string doctorId);
        Task<int> GetPatientPendingConsultationCount(string patientId);
        Task<int> GetPatientCompletedConsultationCount(string patientId);
    }
}

