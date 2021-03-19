using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Surgery")]
    [ApiController]
    public class SurgeryController : Controller
    {
        private readonly IDoctorAppointment _appointment;
        private readonly IConsultation _consultation;
        private readonly ISurgery _surgery;
        

        public SurgeryController(IDoctorAppointment appointment, IConsultation consultation, ISurgery surgery, IMapper mapper)
        {
            _appointment = appointment;
            _consultation = consultation;
            _surgery = surgery;
        }

        [HttpGet("GetSurgeryByConsulatatonOrAppointment")]
        public async Task<IActionResult> GetSurgery(string Id)
        {
            if (Id == "")
            {
                return BadRequest();
            }

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
         

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var surgery = await _surgery.GetSurgeryByAppointmentOrConsultation(Id);

            if (surgery == null)
            {
                return BadRequest(new { response = "301", message = "The Id Passed Has No Surgery Associated With It" });
            }

            return Ok(new { surgery, mwessage = "Ward returned" });
        }



        [HttpPost("UpdateOperationNoteOne")]
        public async Task<IActionResult> UpdateOperationNoteOne(string Id, string IdType, OperationNoteOneDtoForUpdate Surgery)
        {

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
            if (Surgery == null || Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var surgery = await _surgery.GetSurgeryByAppointmentOrConsultation(Id);

            if (surgery == null)
            {
                surgery = await _surgery.CreateSurgery(Id, IdType, Surgery.DoctorId, Surgery.PatientId);
            }


            surgery.HospitalistService = Surgery.HospitalistService;
            surgery.MedSurgService = Surgery.MedSurgService;
            surgery.ICU = Surgery.ICU;
            surgery.TELE = Surgery.TELE;
            surgery.SurgeryAndDiagnosis = Surgery.SurgeryAndDiagnosis;
            surgery.SecondaryDiagnosis = Surgery.SecondaryDiagnosis;
            surgery.Allergies = Surgery.Allergies;
            surgery.OnChat = Surgery.OnChat;
            surgery.CompletedByPCPCall = Surgery.CompletedByPCPCall;
            surgery.Dietary = Surgery.Dietary;
            surgery.VSEveryFourHours = Surgery.VSEveryFourHours;
            surgery.VSEveryEightHours = Surgery.VSEveryEightHours;
            surgery.VSPerUnitProtocol = Surgery.VSPerUnitProtocol;
            surgery.IAndDWeightDaily = Surgery.IAndDWeightDaily;
            surgery.BedRest = Surgery.BedRest;
            surgery.OOBToChain = Surgery.OOBToChain;
            surgery.AMBAsTol = Surgery.AMBAsTol;
            surgery.ManagementPerPDHPolicy = Surgery.ManagementPerPDHPolicy;
            surgery.JacksonPratt = Surgery.JacksonPratt;
            surgery.Hamovac = Surgery.Hamovac;
            surgery.Penrose = Surgery.Penrose;
            surgery.Dressing = Surgery.Dressing;
            surgery.HRLowerLimit = Surgery.HRLowerLimit;
            surgery.HRUpperLimit = Surgery.HRUpperLimit;
            surgery.RPLowerLimit = Surgery.RPLowerLimit;
            surgery.RPUpperLimit = Surgery.RPUpperLimit;
            surgery.SBPLowerLimit = Surgery.SBPLowerLimit;
            surgery.SBPUpperLimit = Surgery.SBPUpperLimit;
            surgery.TemperatureLowerLimit = Surgery.TemperatureLowerLimit;
            surgery.TemperatureUpperLimit = Surgery.TemperatureUpperLimit;
            surgery.DPBLowerLimit = Surgery.DPBLowerLimit;
            surgery.DPBUpperLimit = Surgery.DPBUpperLimit;
            surgery.SPO2LowerLimit = Surgery.SPO2LowerLimit;
            surgery.SPO2UpperLimit = Surgery.SPO2UpperLimit;
            surgery.UrineOutput = Surgery.UrineOutput;
            surgery.Haemoglobin = Surgery.Haemoglobin;
            surgery.UnusualWoundDrainage = Surgery.UnusualWoundDrainage;
            surgery.Pantoprazole = Surgery.Pantoprazole;
            surgery.Famotidine = Surgery.Famotidine;
            surgery.InfectionPrevention = Surgery.InfectionPrevention;
            surgery.RespiratoryCare = Surgery.RespiratoryCare;
           
             var res = await _surgery.UpdateSurgery(surgery);

         
            if (res)
            {
                return Ok(new { message = "Surgery Updated Succesfully" });
            }
            return BadRequest(new { message = "Something Went Wrong" });


        }

        [HttpPost("UpdateOperationNoteTwo")]
        public async Task<IActionResult> UpdateOperationNoteTwo(string Id, string IdType, OperationNoteTwoDtoForUpdate Surgery)
        {

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
            if (Surgery == null || Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var surgery = await _surgery.GetSurgeryByAppointmentOrConsultation(Id);

            if (surgery == null)
            {
                surgery = await _surgery.CreateSurgery(Id, IdType, Surgery.DoctorId, Surgery.PatientId);
            }


            surgery.PostOperationMedication = Surgery.PostOperationMedication;
            surgery.Surgeons = Surgery.Surgeons;
            surgery.Anasthetics = Surgery.Anasthetics;
            surgery.Operation = Surgery.Operation;
            surgery.SurgeryIndication = Surgery.SurgeryIndication;
            surgery.DoctorId = Surgery.DoctorId;
            surgery.PatientId = Surgery.PatientId;

            var res = await _surgery.UpdateSurgery(surgery);

            
            if (res)
            {
                return Ok(new { message = "Surgery Updated Succesfully" });
            }

            return BadRequest(new { message = "Something Went Wrong" });
        }

        [HttpPost("UpdateOperationProcedure")]
        public async Task<IActionResult> UpdateOperationProcedure(string Id, string IdType, OperationProcedureDtoForUpdate Surgery)
        {

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
            if (Surgery == null || Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var surgery = await _surgery.GetSurgeryByAppointmentOrConsultation(Id);

            if (surgery == null)
            {
                surgery = await _surgery.CreateSurgery(Id, IdType, Surgery.DoctorId, Surgery.PatientId);
            }


            surgery.OperationProcedure = Surgery.OperationProcedure;
            surgery.DoctorId = Surgery.DoctorId;
            surgery.PatientId = Surgery.PatientId;

            var res = await _surgery.UpdateSurgery(surgery);

            if (res)
            {
                return Ok(new { message = "Surgery Updated Succesfully" });
            }
            return BadRequest(new { message = "Something Went Wrong" });
        }
    }
}
