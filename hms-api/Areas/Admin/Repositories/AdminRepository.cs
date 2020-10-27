using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using HMS.Models;
using HMS.Models.Doctor;
using HMS.Models.Patient;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public AdminRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public Task<(bool, string)> AddNewPatient()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BookAppointment(DoctorAppointment appointment)
        {
            try
            {
                _applicationDbContext.DoctorAppointments.Add(appointment);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }       
        }

        public async Task<File> GenerateFileNumber(FileDtoForCreate fileToCreate)
        {
            try
            {
                if (fileToCreate != null)
                {
                    var file = _mapper.Map<File>(fileToCreate);

                    _applicationDbContext.Files.Add(file);
                    await _applicationDbContext.SaveChangesAsync();

                    return file;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }         
        }


        public async Task<Models.Account> GetAccountById(int Id)
        {
            try
            {
                if(Id == 0)
                {
                    return null;
                }

                var account = await _applicationDbContext.Accounts.FindAsync(Id);

                return account;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Models.Account>> GetAllAccounts(PaginationParameter pagination)
        {
             var result = _applicationDbContext.Accounts.Where(x => x.IsActive == true).AsQueryable();

            return PagedList<Models.Account>.Create(result, pagination.PageSize, pagination.PageNumber);
        }
        

        public async Task<IEnumerable<ApplicationUser>> GetAllDoctors() =>
            await _applicationDbContext.ApplicationUsers.Where(d => d.UserType == "Doctor").ToListAsync();

        public async Task<DoctorProfile> GetDoctorsById(string Id)
        {
            return await _applicationDbContext.DoctorProfiles.Where(p => p.DoctorId == Id).FirstAsync();
        }

        public async Task<dynamic> GetDoctorsPatientAppointment()
        {
            var doctorAppointments = await _applicationDbContext.DoctorAppointments

             .Join(
                 _applicationDbContext.ApplicationUsers,
                 appointment => appointment.PatientId,
                 applicationUser => applicationUser.Id,
                 (appointment, patient) => new { appointment, patient }
             )
             .Join(
                 _applicationDbContext.ApplicationUsers,
                  appointment => appointment.appointment.DoctorId,
                 applicationUser => applicationUser.Id,
                 (appointment, doctor) => new { appointment.appointment, appointment.patient, doctor }
             )
             .ToListAsync();

            return doctorAppointments;                        
        }

        public async Task<IEnumerable<PatientProfile>> GetPatientsInAccount(int acctId)
        {
            try
            {
                var patients = await _applicationDbContext.PatientProfiles.Where(x => x.AccountId == acctId).ToListAsync();

                return patients;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public async Task<Models.Account> InsertAccount(Models.Account account)
        {
            try
            {
                if(account == null)
                {
                    return null;
                }

                _applicationDbContext.Accounts.Add(account);
                await _applicationDbContext.SaveChangesAsync();

                return account;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertPatient(PatientProfile patient)
        {
            if(patient == null)
            {
                return false;
            }

            _applicationDbContext.PatientProfiles.Add(patient);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
