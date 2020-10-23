﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Patient.PatientQueueViewModel;

namespace HMS.Services.Interfaces.Patient
{
    public interface IPatientQueue
    {
        Task<bool> AddPatientToQueueAsync(AddPatientToQueueViewModel PatientQueue);
        Task <object> GetPatientQueue();
        Task<int> CancelPatientConsultationAsync(string patientQueueId);
        Task<int> CompletePatientConsultationAsync(string patientQueueId);
        Task<int> ExpirePatientConsultationAsync(string patientQueueId);
    }
}
