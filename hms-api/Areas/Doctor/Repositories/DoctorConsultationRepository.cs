using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorConsultationRepository : IDoctorConsultation
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DoctorConsultationRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public PagedList<ConsultationDtoForView> GetConsultationsCompleted(string DoctorId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(a => a.IsCompleted == true).Where(d => d.DoctorId == DoctorId).Include(a => a.Patient).OrderByDescending(c => c.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ConsultationDtoForView> GetConsultationsOnOpenList(string DoctorId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(d => d.DoctorId == null).Include(a => a.Patient).OrderByDescending(c => c.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ConsultationDtoForView> GetConsultationsWithDoctor(string DoctorId, PaginationParameter paginationParameter)
        {
            var consultations = _applicationDbContext.Consultations.Where(d => d.DoctorId == DoctorId).Include(a => a.Patient).OrderByDescending(c => c.DateOfConsultation).ToList();
            var consultationsToReturn = _mapper.Map<IEnumerable<ConsultationDtoForView>>(consultations);
            return PagedList<ConsultationDtoForView>.ToPagedList(consultationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
