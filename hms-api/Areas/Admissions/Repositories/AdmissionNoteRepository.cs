using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class AdmissionNoteRepository : IAdmissionNote
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public AdmissionNoteRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateAdmissionNote(AdmissionNote AdmissionNote)
        {
            try
            {
                if (AdmissionNote == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionNotes.Add(AdmissionNote);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AdmissionNote> GetAdmissionNote(string AdmissionNoteId) => await _applicationDbContext.AdmissionNotes.Where(p => p.Id == AdmissionNoteId).Include(a => a.Admission).ThenInclude(a => a.Patient).Include(a => a.Doctor).FirstOrDefaultAsync();
     

        public PagedList<AdmissionNoteDtoForView> GetAdmissionNotes(string AdmissionId, PaginationParameter paginationParameter)
        {
            var admissionNotes = _applicationDbContext.AdmissionNotes.Where(p => p.AdmissionId == AdmissionId).Include(a => a.Admission).ThenInclude(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateGenerated).ToList();
            var admissionNotesToReturn = _mapper.Map<IEnumerable<AdmissionNoteDtoForView>>(admissionNotes);
            return PagedList<AdmissionNoteDtoForView>.ToPagedList(admissionNotesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}

