using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using AutoMapper;
using System.Linq;
using HMS.Services.Helpers;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugRepository : IDrug
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DrugRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<int> GetDrugCount() => await _applicationDbContext.Drugs.CountAsync();
        public async Task<Drug> GetDrug(string Id) => await _applicationDbContext.Drugs.FindAsync(Id);

        public async Task<IEnumerable<Drug>> GetDrugs() => await _applicationDbContext.Drugs.ToListAsync();

        public PagedList<Drug> GetDrugsPagination(PaginationParameter paginationParameter)
        {
            var drugs = _applicationDbContext.Drugs.ToList();
            var drugsToReturn = _mapper.Map<IEnumerable<Drug>>(drugs);
            return PagedList<Drug>.ToPagedList(drugsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<Drug> GetDrugsByDrugType(string drugType, PaginationParameter paginationParameter)
        {
            var drugs = _applicationDbContext.Drugs.Where(x=>x.DrugType == drugType.ToUpper()).ToList();
            var drugsToReturn = _mapper.Map<IEnumerable<Drug>>(drugs);
            return PagedList<Drug>.ToPagedList(drugsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<IEnumerable<DrugBatch>> GetExpiredDrugs(DateTime date) => await _applicationDbContext.DrugBatches.Include(d => d.Drug).Where(d => d.ExpiryDate <= date).ToListAsync();
        public async Task<IEnumerable<Drug>> SearchDrugs(string searchString)
        {
            
            var drugs = await _applicationDbContext.Drugs.Where(d => d.SKU.Contains(searchString) || d.Name.Contains(searchString) || d.GenericName.Contains(searchString)).ToListAsync();
            return drugs;
        }
        public async Task<bool> CreateDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }
                drug.DrugType = drug.DrugType.ToUpper();

                _applicationDbContext.Drugs.Add(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }

                _applicationDbContext.Drugs.Update(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDrug(Drug drug)
        {
            try
            {
                if (drug == null)
                {
                    return false;
                }

                _applicationDbContext.Drugs.Remove(drug);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckIfDrugsExist(List<string> drugIds)
        {
            if (drugIds == null)
                return false;

            var idNotInDrugs = drugIds.Where(x => _applicationDbContext.Drugs.Any(y => y.Id == x));

            return idNotInDrugs.Any();
        }

       
    }
}
