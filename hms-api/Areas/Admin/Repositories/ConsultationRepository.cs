using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ConsultationRepository : IConsultation
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public ConsultationRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        
        public async Task<int> GetConsultationCount() => await _applicationDbContext.Consultations.Where(c => c.DateOfConsultation.Date == DateTime.Now.Date).CountAsync();

        public async Task<int> GetPatientsUnattendedToCount() => await _applicationDbContext.Consultations.Where(c => c.IsCompleted == false).CountAsync();

        public async Task<int> GetPatientsAttendedToCount() => await _applicationDbContext.Consultations.Where(c => c.IsCompleted == true).CountAsync();

        public async Task<bool> BookConsultation(Consultation consultation)
        {
            try
            {
                _applicationDbContext.Consultations.Add(consultation);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<dynamic> GetConsultations()
        {
            var consultations = await _applicationDbContext.Consultations.Include(a => a.Patient).Include(a => a.Doctor).ToListAsync();
            return consultations;
        }


        public async Task<int> CancelPatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsExpired == true)
            {
                return 2;
            }
            else if (Consultation.IsCompleted == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsCanceled = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> CompletePatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsExpired == true)
            {
                return 2;
            }
            else if (Consultation.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsCompleted = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> ExpirePatientConsultationAsync(string consultationId)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == consultationId);


            if (Consultation == null)
            {
                return 1;
            }
            else if (Consultation.IsCompleted == true)
            {
                return 2;
            }
            else if (Consultation.IsCanceled == true)
            {
                return 3;
            }
            else
            {
                Consultation.IsExpired = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<Consultation> GetConsultationById(string Id) 
        {
            var consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == Id);
            return consultation;
        }

        public async Task<bool> ReassignPatientToNewDoctor(Consultation consultation, JsonPatchDocument<ConsultationDtoForUpdate> Consultation)
        {
            try
            {


                if (consultation != null)
                {

                    var consultationToUpdate = _mapper.Map<ConsultationDtoForUpdate>(consultation);

                    Consultation.ApplyTo(consultationToUpdate);
                    if (consultation != null)
                    {
                        // detach
                        _applicationDbContext.Entry(consultation).State = EntityState.Detached;
                    }

                    consultation = _mapper.Map<Consultation>(consultationToUpdate);

                    _applicationDbContext.Consultations.Update(consultation);
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
