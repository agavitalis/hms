using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class SurgeryRepository : ISurgery
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public SurgeryRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateSurgery(Surgery surgery)
        {
            try
            {
                if (surgery == null)
                {
                    return false;
                }

                _applicationDbContext.Surgeries.Add(surgery);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Surgery> CreateSurgery(string Id, string IdType, string DoctorId, string PatientId)
        {
            try
            {
                Surgery newSurgery = null;

                if (IdType.ToLower() == "appointment")
                {
                    newSurgery = new Surgery()
                    {
                        AppointmentId = Id,
                        DoctorId = DoctorId,
                        PatientId = PatientId
                    };

                    _applicationDbContext.Surgeries.Add(newSurgery);
                    await _applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    newSurgery = new Surgery()
                    {
                        ConsultationId = Id,
                        DoctorId = DoctorId,
                        PatientId = PatientId
                    };

                    _applicationDbContext.Surgeries.Add(newSurgery);
                    await _applicationDbContext.SaveChangesAsync();
                }


                return newSurgery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Surgery> GetSurgeryByAppointmentOrConsultation(string Id) => await _applicationDbContext.Surgeries.Where(s => s.ConsultationId == Id || s.AppointmentId == Id).FirstOrDefaultAsync();


        public async Task<bool> UpdateSurgery(Surgery surgery)
        {
            try
            {
                if (surgery != null)
                {

                    

                    _applicationDbContext.Surgeries.Update(surgery);


                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
