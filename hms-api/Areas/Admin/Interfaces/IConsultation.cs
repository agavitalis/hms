using HMS.Models;
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
        Task<int> GetPatientsUnattendedToCount();
        Task<int> GetPatientsAttendedToCount();
        Task<dynamic> GetConsultations();
        Task<bool> BookConsultation(Consultation patientConsultation);
        Task<int> CancelPatientConsultationAsync(string consultationId);
        Task<int> CompletePatientConsultationAsync(string consultationId);
        Task<int> ExpirePatientConsultationAsync(string consultationId);
    }
}

