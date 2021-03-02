using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IMedication
    {
        Task<bool> CreateMedication(AdmissionMedication admission);
        PagedList<MedicationDtoForView> GetMedications(string AdmissionId, PaginationParameter paginationParameter);
    }
}
