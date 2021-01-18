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

    public class ServiceRequestRepository : IServiceRequest
    {
        private readonly ApplicationDbContext _applicationDbContext;
       

        public ServiceRequestRepository(ApplicationDbContext applicationDbContext)
        {
            
            _applicationDbContext = applicationDbContext;
           
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestByServiceAsync(string ServiceId) => await _applicationDbContext.ServiceRequests.Where(s => s.ServiceId == ServiceId).ToListAsync();


        public async Task<int> GetServiceRequestPaidAndDoneCount()
        {
            var complatedServices = await _applicationDbContext.ServiceRequests.Where(s => s.Status == "DONE" && s.PaymentStatus == "PAID").CountAsync();

            return complatedServices;
        }


        public async Task<int> GetServiceRequestPaidAndNotDoneCount()
        {
            var pendingServices = await _applicationDbContext.ServiceRequests.Where(s => s.Status == "UNDONE" && s.PaymentStatus == "PAID").CountAsync();

            return pendingServices;
        }
    }
}