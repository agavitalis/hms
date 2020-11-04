using HMS.Database;
using HMS.Areas.Doctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorAppointmentRepository : IDoctorAppointment
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DoctorAppointmentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Appointment> GetAppointment(string Id)
        {
            return await _applicationDbContext.DoctorAppointments.Where(a => a.Id == Id).FirstOrDefaultAsync();
        }

        Task<Appointment> IDoctorAppointment.GetAppointment(string Id)
        {
            throw new System.NotImplementedException();
        }
    }
}
