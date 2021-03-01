using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admissions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Invoices")]
    [ApiController]
    public class AdmissionInvoiceController : Controller
    {
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IMapper _mapper;

        public AdmissionInvoiceController(IAdmissionInvoice admissionInvoice, IMapper mapper)
        {
            _admissionInvoice = admissionInvoice;
            _mapper = mapper;
        }

        [Route("GetAdmissionInvoiceForAdmission")]
        [HttpGet]
        public async Task<IActionResult> GetAdmissionInvoicesForAdmission(string AdmissionId)
        {
            var admissionInvoice = await _admissionInvoice.GetAdmissionInvoiceByAdmissionId(AdmissionId);

            return Ok(new
            {
                admissionInvoice,
                message = "Admission Invoice Returned"
            });
        }
    }
}
