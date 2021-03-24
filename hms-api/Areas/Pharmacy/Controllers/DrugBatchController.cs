using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy", Name = "Pharmacy - Manage Drug Inventory")]
    [ApiController]
    public class DrugBatchController : Controller
    {
        private readonly IDrug _drug;
        private readonly IDrugBatch _drugBatch;
        private readonly IMapper _mapper;
        public DrugBatchController(IDrugBatch drugBatch, IDrug drug, IMapper mapper)
        {
            _drugBatch = drugBatch;
            _drug = drug;
            _mapper = mapper;
        }

        [HttpGet("GetDrugBatch/{Id}")]
        public async Task<IActionResult> GetWardById(string DrugBatchId)
        {
            if (DrugBatchId == "")
            {
                return BadRequest();
            }

            var res = await _drugBatch.GetDrugBatch(DrugBatchId);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Drug Batch returned" });
        }



        [HttpGet("GetDrugBatchByDrug")]
        public async Task<IActionResult> GetDrugBatchByDrug([FromQuery] PaginationParameter paginationParameter, string DrugBatchId)
        {
            var drugBatch = _drugBatch.GetDrugBatchByDrug(DrugBatchId, paginationParameter);


            var paginationDetails = new
            {
                drugBatch.TotalCount,
                drugBatch.PageSize,
                drugBatch.CurrentPage,
                drugBatch.TotalPages,
                drugBatch.HasNext,
                drugBatch.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                drugBatch,
                paginationDetails,
                message = "Drug Batch Fetched"
            });

        }


        [HttpPost("CreateDrugBatch")]
        public async Task<IActionResult> CreateDrugBatch(DrugBatchDtoForCreate drugBatch)
        {
            if (drugBatch == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugBatchToCreate = _mapper.Map<DrugBatch>(drugBatch);

            var res = await _drugBatch.CreateDrugBatch(drugBatchToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Drug Batch failed to create" });
            }

            return Ok(new
            {
                drugBatch,
                message = "Drug Batch created successfully"
            });
        }




        [HttpPost("UpdateDrugBatch")]
        public async Task<IActionResult> UpdateDrugBatch(DrugBatchDtoForUpdate drugBatch)
        {
            if (drugBatch == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugBatchToUpdate = _mapper.Map<DrugBatch>(drugBatch);

            var res = await _drugBatch.UpdateDrugBatch(drugBatchToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to update" });
            }

            return Ok(new
            {
                drugBatch,
                message = "Drug Batch updated successfully"
            });
        }

        [HttpPost("DeleteDrugBatch")]
        public async Task<IActionResult> DeleteDrugBatch(DrugBatchDtoForDelete drugBatch)
        {
            if (drugBatch == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var drugBatchToDelete = _mapper.Map<DrugBatch>(drugBatch);

            var res = await _drugBatch.DeleteDrugBatch(drugBatchToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to delete" });
            }

            return Ok(new { drugBatch, message = "Ward Deleted" });
        }
    }

}

