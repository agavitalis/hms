using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugBatchRepository : IDrugBatch
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public DrugBatchRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateDrugBatch(DrugBatch DrugBatch)
        {
            try
            {
                if (DrugBatch == null)
                {
                    return false;
                }
               

                _applicationDbContext.DrugBatches.Add(DrugBatch);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDrugBatch(DrugBatch DrugBatch)
        {
            try
            {
                if (DrugBatch == null)
                {
                    return false;
                }


                _applicationDbContext.DrugBatches.Remove(DrugBatch);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DrugBatch> GetDrugBatch(string DrugBatchId) => await _applicationDbContext.DrugBatches.Where(d => d.Id == DrugBatchId).FirstOrDefaultAsync();
       

        public async Task<DrugBatch> GetDrugBatchByDrug(string DrugId, int quantityOfDrugs) => await _applicationDbContext.DrugBatches.Where(d => d.DrugId == DrugId && d.IsActive == true && d.QuantityInStock >= quantityOfDrugs).FirstOrDefaultAsync();

        public PagedList<DrugBatchDtoForView> GetDrugBatchByDrug(string DrugId, PaginationParameter paginationParameter)
        {
            var drugBatches = _applicationDbContext.DrugBatches.Include(d => d.Drug).OrderByDescending(d => d.ExpiryDate).ToList();

            var drugBatchesToReturn = _mapper.Map<IEnumerable<DrugBatchDtoForView>>(drugBatches);

            return PagedList<DrugBatchDtoForView>.ToPagedList(drugBatchesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }


        public async Task<bool> UpdateDrugBatch(DrugBatch DrugBatch)
        {
            try
            {
                if (DrugBatch == null)
                {
                    return false;
                }


                _applicationDbContext.DrugBatches.Update(DrugBatch);
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
