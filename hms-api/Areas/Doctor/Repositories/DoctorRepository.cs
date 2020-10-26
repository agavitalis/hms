using HMS.Database;
using HMS.Models.Doctor;
using HMS.Areas.Doctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorRepository : IDoctor
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DoctorRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<DoctorAppointment> GetAppointment(string Id)
        {
            return await _applicationDbContext.DoctorAppointments.Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
    }
}
