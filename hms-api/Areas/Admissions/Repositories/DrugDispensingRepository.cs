using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace HMS.Areas.Admissions.Repositories
{
    public class DrugDispensingRepository : IAdmissionDrugDispensing
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DrugDispensingRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
      

        public PagedList<AdmissionDrugDispensingDtoForView> GetAdmissionDrugDispensing(string InvoiceId, PaginationParameter paginationParameter)
        {
            var drugDispensing = _applicationDbContext.AdmissionDrugDispensings.Where(a => a.AdmissionInvoiceId == InvoiceId).Include(a => a.Drug).OrderByDescending(a => a.DateDispensed).ToList();

            var drugDispensingToReturn = _mapper.Map<IEnumerable<AdmissionDrugDispensingDtoForView>>(drugDispensing);

            return PagedList<AdmissionDrugDispensingDtoForView>.ToPagedList(drugDispensingToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
