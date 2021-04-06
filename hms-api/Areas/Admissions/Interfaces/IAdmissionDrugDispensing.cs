using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IAdmissionDrugDispensing
    {
       PagedList<AdmissionDrugDispensingDtoForView> GetAdmissionDrugDispensing(string InvoiceId, PaginationParameter paginationParameter);
    }
}
