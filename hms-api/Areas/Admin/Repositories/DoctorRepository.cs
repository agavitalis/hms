﻿using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class DoctorRepository : IDoctor
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DoctorRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllDoctors() =>
            await _applicationDbContext.ApplicationUsers.Where(d => d.UserType == "Doctor").ToListAsync();

        public async Task<DoctorProfile> GetDoctorsById(string Id)
        {
            return await _applicationDbContext.DoctorProfiles.Where(p => p.DoctorId == Id).FirstAsync();
        }

     
    }
}
