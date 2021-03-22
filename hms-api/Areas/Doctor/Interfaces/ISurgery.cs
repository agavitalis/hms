using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface ISurgery
    {
        Task<Surgery> GetSurgery(string SurgeryId);
        PagedList<SurgeryDtoForView> GetSurgeries(PaginationParameter paginationParameter);
        PagedList<SurgeryDtoForView> GetSurgeriesByAppointmentOrConsultation(string Id, PaginationParameter paginationParameter);
        Task<bool> CreateSurgery(string Id, string IdType, string InitiatorId, string PatientId, string ReferralNote, DateTime DateOfSurgery, DateTime TimeOfSurgery);
        Task<bool> CreateSurgery(string InitiatorId, string PatientId, string ReferralNote, DateTime DateOfSurgery, DateTime TimeOfSurgery);
        Task<bool> UpdateSurgery(Surgery surgery);
    }
}
