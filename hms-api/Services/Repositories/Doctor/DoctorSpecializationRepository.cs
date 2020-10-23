using HMS.Database;
using HMS.Models;
using HMS.Models.Doctor;
using HMS.Services.Interfaces.Doctor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.ViewModels.Doctor.DoctorSpecializationViewModel;

namespace HMS.Services.Repositories.Doctor
{
    public class DoctorSpecializationRepository : IDoctorSpecialization
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DoctorSpecializationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<object> GetDoctorSpecializationAsync(string DoctorId)
        {
            var DoctorSpecialization = _applicationDbContext.DoctorSpecializations.Where(p => p.DoctorId == DoctorId)
                       .Join(
                           _applicationDbContext.ApplicationUsers,
                           doctorSpecialization => doctorSpecialization.DoctorId,
                           applicationUser => applicationUser.Id,
                           (doctorSpecialization, applicationUser) => new { doctorSpecialization, applicationUser }
                       ).ToListAsync();
                       

            return await DoctorSpecialization;

        }

        public async Task<bool> CreateDoctorSpecializationAsync(CreateDoctorSpecializationViewModel CreateDoctorSpecialization)
        {
            var specialization = new DoctorSpecialization()
            {
                Name = CreateDoctorSpecialization.Name,
                DoctorId = CreateDoctorSpecialization.DoctorId,
            };
            _applicationDbContext.DoctorSpecializations.Add(specialization);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
           
        public async Task<bool> EditDoctorSpecializationAsync(EditDoctorSpecializationViewModel EditDoctorSpecialization)
        {
            //check if this guy has a profile already
            var DoctorSpecialization = await _applicationDbContext.DoctorSpecializations.FirstOrDefaultAsync(d => d.Id == EditDoctorSpecialization.Id);

            DoctorSpecialization.Name = EditDoctorSpecialization.Name;        
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDoctorSpecializationAsync(string SpecializationId)
        {

            // Retrieve Doctor by id
            var Specialization = await _applicationDbContext.DoctorSpecializations.FirstOrDefaultAsync(d => d.Id == SpecializationId);
            
            // Validate Doctor selected is not null
            if (Specialization != null)
            {
                //Delete Doctor From Database
                _applicationDbContext.DoctorSpecializations.Remove(Specialization);

                // Save changes in database
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
