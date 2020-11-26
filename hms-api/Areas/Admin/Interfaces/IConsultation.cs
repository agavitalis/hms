﻿using HMS.Areas.Admin.Dtos;
using HMS.Models;
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
        Task<int> GetPatientsUnattendedToCount();
        Task<int> GetPatientsAttendedToCount();
        Task<dynamic> GetConsultations();
        Task<Consultation> GetConsultationById(string Id);
        Task<bool> BookConsultation(Consultation patientConsultation);
        Task<bool> DeleteConsultation(Consultation consultation);
        Task<int> CancelPatientConsultationAsync(string consultationId);
        Task<int> CompletePatientConsultationAsync(string consultationId);
        Task<int> ExpirePatientConsultationAsync(string consultationId);
        Task<bool> ReassignPatientToNewDoctor(Consultation consultation, JsonPatchDocument<ConsultationDtoForUpdate> Consultation);
    }
}

