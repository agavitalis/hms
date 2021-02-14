using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Dtos;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<int> AdmitPatientOrSendPatientHome(CompletDoctorClerkingDto clerking)
        {
            //check if the patient is in queue today
            var Consultation = await _applicationDbContext.Consultations.FirstOrDefaultAsync(d => d.Id == clerking.Id);


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
                Consultation.IsPatientAdmitted = clerking.IsAdmitted;
                Consultation.IsPatientSentHome = clerking.IsSentHome;
               
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<bool> UpdateConsultation(Consultation consultation)
        {
            try
            {
                if (consultation == null)
                {
                    return false;
                }

                _applicationDbContext.Consultations.Update(consultation);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public async Task<bool> DeleteConsultation(Consultation consultation)
        {
            try
            {
                if (consultation == null)
                {
                    return false;
                }

                _applicationDbContext.Consultations.Remove(consultation);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetDoctorsPendingConsultationCount(string doctorId)
        {
            var doctorPendingConsultationsCount = await _applicationDbContext.Consultations.Where(a => a.IsCompleted == false && a.DoctorId == doctorId).CountAsync();
            return doctorPendingConsultationsCount;
        }

        public async Task<int> GetDoctorsCompletedConsultationCount(string doctorId)
        {
            var doctorCompletedConsultationCount = await _applicationDbContext.Consultations.Where(a => a.IsCompleted == true && a.DoctorId == doctorId).CountAsync();
            return doctorCompletedConsultationCount;
        }

        public async Task<int> GetPatientPendingConsultationCount(string patientId)
        {
            var patientPendingConsultationsCount = await _applicationDbContext.Consultations.Where(a => a.IsCompleted == false && a.PatientId == patientId).CountAsync();
            return patientPendingConsultationsCount;
        }

        public async Task<int> GetPatientCompletedConsultationCount(string patientId)
        {
            var patientCompletedConsultationCount = await _applicationDbContext.Consultations.Where(a => a.IsCompleted == true && a.PatientId == patientId).CountAsync();
            return patientCompletedConsultationCount;
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

        public async Task<MyPatient> CheckDoctorInMyPatients(string DoctorId, string PatientId) => await _applicationDbContext.MyPatients.Where(p => p.DoctorId == DoctorId && p.PatientId == PatientId).FirstOrDefaultAsync();

        public PagedList<ConsultationDtoForView> GetConsultationsPagination(PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Include(a => a.Patient).Include(a => a.Doctor).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ConsultationDtoForView> GetConsultationsOnOpenList(PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(d=>d.DoctorId == null).Include(a => a.Patient).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ConsultationDtoForView> GetConsultationsWithDoctors(PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(d => d.DoctorId != null).Include(a => a.Patient).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ConsultationDtoForView> GetConsultationsCompleted(PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(a => a.IsCompleted == true).Include(a => a.Patient).Include(a => a.Doctor).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

    }
}
