﻿using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorClerkingRepository : IDoctorClerking
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DoctorClerkingRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public async Task<DoctorClerking> CreateDoctorClerking(string Id, string IdType)
        {
            try
            {
                DoctorClerking newClarking = null;

                if(IdType.ToLower() == "appointment")
                {
                     newClarking = new DoctorClerking()
                    {
                        AppointmentId = Id,
                    };

                    _applicationDbContext.DoctorClerkings.Add(newClarking);
                    await _applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    newClarking = new DoctorClerking()
                    {
                        ConsultationId = Id,
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



       
        public async Task<DoctorClerking> GetDoctorClerking(string Id)
        {
           var clerking = await  _applicationDbContext.DoctorClerkings.Where(c => c.ConsultationId == Id ||  c.AppointmentId == Id).FirstOrDefaultAsync();
            return clerking;
        }
        
      
       

        public async Task<bool> UpdateDoctorClerking(DoctorClerking doctorClerking, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking)
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