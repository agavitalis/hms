using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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

        public async Task<bool> CreateSurgery(string Id, string IdType, string InitiatorId, string PatientId, string ReferralNote, DateTime DateOfSurgery, DateTime TimeOfSurgery)
        {
            try
            {
                Surgery newSurgery = null;

                if (IdType.ToLower() == "appointment")
                {
                    newSurgery = new Surgery()
                    {
                        ReferralNote = ReferralNote,
                        AppointmentId = Id,
                        InitiatorId = InitiatorId,
                        PatientId = PatientId,
                        DateOfSurgery = DateOfSurgery,
                        TimeOfSurgery = TimeOfSurgery
                    };

                    _applicationDbContext.Surgeries.Add(newSurgery);
                    await _applicationDbContext.SaveChangesAsync();
                }
                else if (IdType.ToLower() == "consultation")
                {
                    newSurgery = new Surgery()
                    {
                        ReferralNote = ReferralNote,
                        ConsultationId = Id,
                        InitiatorId = InitiatorId,
                        PatientId = PatientId,
                        DateOfSurgery = DateOfSurgery,
                        TimeOfSurgery = TimeOfSurgery
                    };

                    _applicationDbContext.Surgeries.Add(newSurgery);
                    await _applicationDbContext.SaveChangesAsync();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateSurgery(string InitiatorId, string PatientId, string ReferralNote, DateTime DateOfSurgery, DateTime TimeOfSurgery)
        {
            Surgery newSurgery = null;

            
           
                newSurgery = new Surgery()
                {
                    ReferralNote = ReferralNote,
                    DoctorId = InitiatorId,
                    PatientId = PatientId,
                    DateOfSurgery = DateOfSurgery,
                    TimeOfSurgery = TimeOfSurgery
                };

                _applicationDbContext.Surgeries.Add(newSurgery);
                await _applicationDbContext.SaveChangesAsync();
            
            return true;
        }

        public PagedList<SurgeryDtoForView> GetSurgeries(PaginationParameter paginationParameter)
        {
            var surgeries = _applicationDbContext.Surgeries.Include(a => a.Patient).Include(s => s.Doctor).Include(s => s.Initiator).ToList();
            var surgeriesToReturn = _mapper.Map<IEnumerable<SurgeryDtoForView>>(surgeries);
            return PagedList<SurgeryDtoForView>.ToPagedList(surgeriesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<SurgeryDtoForView> GetSurgeriesByAppointmentOrConsultation(string Id, PaginationParameter paginationParameter)
        {
            var surgeries = _applicationDbContext.Surgeries.Where(s => s.ConsultationId == Id || s.AppointmentId == Id).Include(a => a.Patient).Include(s => s.Doctor).Include(s => s.Initiator).ToList();
            var surgeriesToReturn = _mapper.Map<IEnumerable<SurgeryDtoForView>>(surgeries);
            return PagedList<SurgeryDtoForView>.ToPagedList(surgeriesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<Surgery> GetSurgery(string SurgeryId) => await _applicationDbContext.Surgeries.Where(s => s.Id == SurgeryId).FirstOrDefaultAsync();
       

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
