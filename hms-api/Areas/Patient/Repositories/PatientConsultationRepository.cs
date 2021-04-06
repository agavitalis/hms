using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Patient.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;

namespace HMS.Areas.Patient.Repositories
{
    public class PatientConsultationRepository : IPatientConsultation
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        
        public PatientConsultationRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> AssignDoctorToPatient(MyPatient patient)
        {
            try
            {
                _applicationDbContext.MyPatients.Add(patient);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> BookConsultation(BookConsultation consultation)
        {
            //check if this guy has a profile already
            var Patient = await _applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == consultation.PatientId);
            
            // Validate patient is not null---has no profile yet
            if (Patient != null)
            {
                //add patient to queue
                var queue = new Consultation()
                {
                    ConsultationTitle = consultation.ConsultationTitle ,
                    ReasonForConsultation = consultation.ReasonForConsultation,
                    PatientId = consultation.PatientId,
                    DoctorId = consultation.DoctorId
                };

                
                _applicationDbContext.Consultations.Add(queue);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            return false;
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
                Consultation.IsPending = false;
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
                Consultation.IsPending = false;
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
                Consultation.IsPending = false;
                Consultation.IsExpired = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<object> GetAPatientConsultations(string patientId)
        {
       
            var Consultation = _applicationDbContext.Consultations.Where(c => c.PatientId == patientId).Include(c => c.Patient).Include(c => c.Doctor).ToListAsync();
            return await Consultation;

        }

        public PagedList<ConsultationDtoForView> GetCanceledConsultations(string PatientId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(a => a.IsCanceled == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<int> GetCanceledConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCanceled == true).CountAsync();

        public PagedList<ConsultationDtoForView> GetCompletedConsultations(string PatientId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(a => a.IsCompleted == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<int> GetCompletedConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCompleted == true).CountAsync();

        public PagedList<ConsultationDtoForView> GetPendingConsultations(string PatientId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(a => a.IsPending == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<int> GetPendingConsultationsCount(string patientId) => await _applicationDbContext.Consultations.Where(c => c.PatientId == patientId && c.IsCompleted == false).CountAsync();
    }
}
