﻿using HMS.Models;
using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAdmin
    {
        Task<dynamic> GetDoctorsPatientAppointment();
        Task<bool> BookAppointment(DoctorAppointment appointment);
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);

    }
}
