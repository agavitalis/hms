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
    public class PrescriptionRepository : IPrescription
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public PrescriptionRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public PagedList<PrescriptionsDtoForView> GetAdmissionPrescriptions(string AdmissionId, PaginationParameter paginationParameter)
        {
            var prescriptions = _applicationDbContext.AdmissionPrescriptions.Include(a => a.Admission).Include(a => a.Doctor).ToList();
            var prescriptionsToReturn = _mapper.Map<IEnumerable<PrescriptionsDtoForView>>(prescriptions);
            return PagedList<PrescriptionsDtoForView>.ToPagedList(prescriptionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> UpdatePrescriptions(AdmissionPrescription prescription)
        {
            try
            {
                if (prescription == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionPrescriptions.Add(prescription);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
