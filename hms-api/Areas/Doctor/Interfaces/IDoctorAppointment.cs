﻿using HMS.Areas.Doctor.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorAppointment
    {
        Task<DoctorAppointment> GetAppointment(string Id);
    }
}
