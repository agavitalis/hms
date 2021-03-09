using AutoMapper;
using HMS.Areas.Interfaces.Nurse;
using HMS.Areas.Nurse.Dtos;
using HMS.Database;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Nurse.Repositories
{
   

    public class NurseRepository : INurse
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public NurseRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public PagedList<NurseDtoForView> GetNurses(PaginationParameter paginationParameter)
        {
            var nurses = _applicationDbContext.NurseProfiles.Include(n => n.Nurse).ToList();

            var nursesToReturn = _mapper.Map<IEnumerable<NurseDtoForView>>(nurses);

            return PagedList<NurseDtoForView>.ToPagedList(nursesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
