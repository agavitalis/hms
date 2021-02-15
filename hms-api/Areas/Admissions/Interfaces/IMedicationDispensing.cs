﻿using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IMedicationDispensing
    {
        Task<IEnumerable<AdmissionMedicationDispensing>> GetAdmissionMedicationDispensing(string AdmissionId);
        Task<bool> CreateMedicationDispensing(AdmissionMedicationDispensing admissionMedicationDispensing);
    }
}
