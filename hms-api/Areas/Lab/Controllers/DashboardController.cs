using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/Lab", Name = "Lab Attendant - Dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
     
        private readonly IServices _services;
        private readonly IServiceCategory _serviceCategory;
        private readonly IServiceRequest _serviceRequest;

        public DashboardController( IServices services, IServiceCategory serviceCategory,IServiceRequest serviceRequest)
        {
           
            _services = services;
            _serviceCategory = serviceCategory;
            _serviceRequest = serviceRequest;

        }

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> GetSystemCount()
        {
 
           
            var serviceCategoryCount = await _serviceCategory.ServiceCategoryCount();
            var servicesCount = await _services.GetServiceCount();

            var serviceRequestPaidAndDoneCount = await _serviceRequest.GetServiceRequestPaidAndDoneCount();
            var serviceRequestPaidAndNotDoneCount = await _serviceRequest.GetServiceRequestPaidAndNotDoneCount();

            return Ok(new
            {
                serviceCategoryCount,
                servicesCount,
                serviceRequestPaidAndDoneCount,
                serviceRequestPaidAndNotDoneCount,
                message = "Patient Dashboard Counts"
            });

        }

    }
}
