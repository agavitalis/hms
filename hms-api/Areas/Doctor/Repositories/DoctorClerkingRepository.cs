using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorClerkingRepository : IDoctorClerking
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IUser _user;

        public DoctorClerkingRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IUser user)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _user = user;
        }

       

        public async Task<bool> CreateDoctorClerking(DoctorClerking clerking)
        {
            try
            {
                if (clerking == null)
                {
                    return false;
                }

                _applicationDbContext.DoctorClerkings.Add(clerking);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DoctorClerking> CreateDoctorClerking(string Id, string IdType, string UserId, string PatientId)
        {
            try
            {
                DoctorClerking newClarking = null;

                if(IdType.ToLower() == "appointment")
                {
                     newClarking = new DoctorClerking()
                    {
                        AppointmentId = Id,
                        DoctorId = UserId,
                        PatientId = PatientId
                     };

                    _applicationDbContext.DoctorClerkings.Add(newClarking);
                    await _applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    newClarking = new DoctorClerking()
                    {
                        ConsultationId = Id,
                        DoctorId = UserId,
                        PatientId = PatientId
                    };

                    _applicationDbContext.DoctorClerkings.Add(newClarking);
                    await _applicationDbContext.SaveChangesAsync();
                }
               

                return newClarking;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



       
        public async Task<DoctorClerking> GetDoctorClerkingByAppointmentOrConsultation(string Id) => await  _applicationDbContext.DoctorClerkings.Include(c => c.Doctor).Where(c => c.ConsultationId == Id ||  c.AppointmentId == Id).FirstOrDefaultAsync();
        public async Task<IEnumerable<DoctorClerking>> GetClerkings() => await _applicationDbContext.DoctorClerkings.Include(c => c.Doctor).Include(c => c.Patient).ToListAsync();
        public async Task<DoctorClerking> GetClerking(string ClerkingId) => await _applicationDbContext.DoctorClerkings.Where(c => c.Id == ClerkingId).Include(c => c.Doctor).Include(c =>c.Patient).FirstOrDefaultAsync();
        public async Task<IEnumerable<DoctorClerking>> GetDoctorClerkingByPatient(string PatientId) => await _applicationDbContext.DoctorClerkings.Where(c => c.Appointment.PatientId == PatientId || c.Consultation.PatientId == PatientId).Include(c => c.Doctor).ToListAsync();
        public async Task<DoctorClerking> GetDoctorClerkingByAppointment(string AppointmentId) => await _applicationDbContext.DoctorClerkings.Where(c => c.AppointmentId == AppointmentId).Include(c => c.Appointment.Doctor).FirstOrDefaultAsync();
        public async Task<DoctorClerking> GetDoctorClerkingByConsultation(string ConsultationId) => await _applicationDbContext.DoctorClerkings.Where(c => c.ConsultationId == ConsultationId).Include(c => c.Consultation.Doctor).FirstOrDefaultAsync();

        public async Task<bool> UpdateDoctorClerking(string UserId, DoctorClerking doctorClerking, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking)
        {
            try
            {
                
                
                if (doctorClerking != null)
                {
            
                    var clerkingToUpdate = _mapper.Map<DoctorClerkingDtoForUpdate>(doctorClerking);
     
                    clerking.ApplyTo(clerkingToUpdate);
                    if (doctorClerking != null)
                    {
                        // detach
                        _applicationDbContext.Entry(doctorClerking).State = EntityState.Detached;
                    }

                    doctorClerking = _mapper.Map<DoctorClerking>(clerkingToUpdate);

                    _applicationDbContext.DoctorClerkings.Update(doctorClerking);
                    //if (UserId != null)
                    //{
                    //    var user = await _user.GetUserByIdAsync(UserId);
                    //    if (user.UserType == "Admin")
                    //    {
                    //        //doctorClerking.Consultation.DoctorId = UserId;
                    //        _applicationDbContext.Consultations.Update(doctorClerking.Consultation);
                    //    }
                    //}
                   
                    
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}
