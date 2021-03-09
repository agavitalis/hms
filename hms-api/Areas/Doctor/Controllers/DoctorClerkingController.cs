using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Clerking")]
    [ApiController]
    public class DoctorClerkingController : Controller
    {
        private readonly IDoctorClerking _clerking;
        private readonly IMapper _mapper;
        private readonly IDoctorAppointment _appointment;
        private readonly IConsultation _consultation;
        private readonly IPatientProfile _patient;
        private readonly IPatientPreConsultation _patientPreConsultation;
        private readonly IEmailSender _emailSender;
        private readonly IAdmission _admission;
        private readonly IAdmissionInvoice _admissionInvoice;
        private readonly IAdmissionServiceRequest _admissionRequest;
        public DoctorClerkingController(IDoctorClerking clerking, IMapper mapper, IDoctorAppointment appointment, IConsultation consultation, IPatientProfile patient, IPatientPreConsultation patientPreConsultation, IEmailSender emailSender, IAdmission admission, IAdmissionInvoice admissionInvoice, IAdmissionServiceRequest admissionRequest)
        {
            _clerking = clerking;
            _mapper = mapper;
            _appointment = appointment;
            _consultation = consultation;
            _patient = patient;
            _patientPreConsultation = patientPreConsultation;
            _emailSender = emailSender;
            _admission = admission;
            _admissionInvoice = admissionInvoice;
            _admissionRequest = admissionRequest;
        }

        [Route("GetClerkings")]
        [HttpGet]
        public async Task<IActionResult> GetClerkings()
        {
            var clerkings = await _clerking.GetClerkings();

            return Ok(new
            {
                clerkings,
                message = "Clerking Returned"
            });
        }

        [Route("GetClerking")]
        [HttpGet]
        public async Task<IActionResult> GetClerking(string ClerkingId)
        {
            var clerking = await _clerking.GetClerking(ClerkingId);

            return Ok(new
            {
                clerking,
                message = "Clerking Returned"
            });
        }
        [Route("GetClerkingHistoryForPatient")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForPatient(string PatientId)
        {
            if (PatientId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var patient = await _patient.GetPatientByIdAsync(PatientId);

            if (patient == null)
            {
                return NotFound();
            }

            var clerkingHistory = await _clerking.GetDoctorClerkingByPatient(PatientId);

            return Ok(new
            {
                clerkingHistory,
                message = "Clerking History returned" 
            });
        }

        [Route("GetClerkingForAppointment")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForAppointment(string AppointmentId)
        {
            if (AppointmentId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var appoinmtent = await _appointment.GetAppointmentById(AppointmentId);

            if (appoinmtent == null)
            {
                return NotFound();
            }

            var clerking = await _clerking.GetDoctorClerkingByAppointment(AppointmentId);

            return Ok(new
            {
                clerking,
                message = "Clerking Returned"
            });
        }

        [Route("GetClerkingForConsultation")]
        [HttpGet]
        public async Task<IActionResult> GetClerkingHistoryForConsultation(string ConsultationId)
        {
            if (ConsultationId == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var consultation = await _consultation.GetConsultationById(ConsultationId);

            if (consultation == null)
            {
                return NotFound();
            }

            var clerking = await _clerking.GetDoctorClerkingByConsultation(ConsultationId);

            return Ok(new
            {
                clerking,
                message = "Clerking Returned"
            });
        }


        [Route("UpdatePatientClerking")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePatientClerking(string Id, string IdType, string UserId, string PatientId, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking)
        {

            var consultation = await _consultation.GetConsultationById(Id);
            var appointment = await _appointment.GetAppointmentById(Id);
            if (clerking == null || Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var doctorClerking = await _clerking.GetDoctorClerkingByAppointmentOrConsultation(Id);

            if (doctorClerking == null)
            {
                doctorClerking = await _clerking.CreateDoctorClerking(Id, IdType, UserId, PatientId);
            }

            //then we patch
            await _clerking.UpdateDoctorClerking(UserId, doctorClerking,  clerking);

            return Ok(new
            {
                clerking,
                message = "Clerking updated successfully"
            });
        }

        [Route("AdmitOrSendPatientHome")]
        [HttpPost]
        public async Task<IActionResult> SendPatientHomeOrAdmit(CompletDoctorClerkingDto Clerking)
        {
            var admissionFee = 10000;
            //Id can either be an appointment or consultation Id
            if (Clerking.IsAdmitted == Clerking.IsSentHome)
            {
                return BadRequest(new { message = "IsSentHome and IsAdmitted Cannot Have The Same Value" });
            }
            if (Clerking.Id == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }


            var consultation = await _consultation.GetConsultationById(Clerking.Id);
            var appointment = await _appointment.GetAppointmentById(Clerking.Id);
            var clerking = await _clerking.GetDoctorClerkingByAppointmentOrConsultation(Clerking.Id);
            
            

            if (consultation == null && appointment == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            if (consultation != null)
            {
                var preconsultation = await _patientPreConsultation.GetPatientPreConsultation(consultation.PatientId);
                var patient = await _patient.GetPatientByIdAsync(consultation.PatientId);
                var patientEmail = patient.Patient.Email;
                var res =  await _consultation.AdmitPatientOrSendPatientHome(Clerking);
                
                if (res == 0 && Clerking.IsAdmitted == true)
                {
                    if (Clerking.AdmissionNote == null)
                    {
                        return BadRequest(new { message = "Admission Must Have an Admission Note" });
                    }
                    
                    consultation.IsCompleted = true;
                    consultation.IsCanceled = false;
                    consultation.IsExpired = false;
                    

                    var admissionToCreate = new Admission()
                    {
                        PatientId = consultation.PatientId,
                        DoctorId = Clerking.InitiatorId,
                        AdmissionNote = Clerking.AdmissionNote,
                        ConsultationId = Clerking.Id

                    };

                    await _admission.CreateAdmission(admissionToCreate);

                    var admissionInvoiceToCreate = new AdmissionInvoice()
                    {
                        Amount = admissionFee,
                        GeneratedBy = Clerking.InitiatorId,
                        AdmissionId = admissionToCreate.Id,
                    };


                    var admissionInvoiceId = await _admissionInvoice.CreateAdmissionInvoice(admissionInvoiceToCreate);

                    if (string.IsNullOrEmpty(admissionInvoiceId))
                    {
                        return BadRequest(new { response = "301", message = "Failed to generate invoice !!!, Try Again" });
                    }

                    var admissionRequestToCreate = new AdmissionServiceRequest()
                    {

                        Amount = admissionFee,
                        AdmissionInvoiceId = admissionInvoiceId,
                    };

                    var result = await _admissionRequest.CreateAdmissionRequest(admissionRequestToCreate);

                    await _consultation.UpdateConsultation(consultation);

                    
                    string emailSubject = "HMS Doctors Report";

                    string emailContent = "<p>" + clerking + " " + preconsultation + "</p>";
                    var message = new EmailMessage(new string[] { patientEmail }, emailSubject, emailContent);
                    _emailSender.SendEmail(message);

                    return Ok( new { message = "Patient Was Admitted" });
                }
                else if (res == 0 && Clerking.IsSentHome == true)
                {
                    consultation.IsCompleted = true;
                    consultation.IsCanceled = false;
                    consultation.IsExpired = false;
                  
                    await _consultation.UpdateConsultation(consultation);

                    string emailSubject = "HMS Doctors Report";
                    
                    string emailContent = "<p>"+ clerking +" "+ preconsultation + "</p>";
                    var message = new EmailMessage(new string[] { patientEmail }, emailSubject, emailContent);
                    _emailSender.SendEmail(message);

                    return Ok(new { message = "Patient Was Sent Home" });
                }
                else if (res == 1)
                {
                    return BadRequest(new { message = "Invalid Consultation Id" });
                }
                else if (res == 2)
                {
                    return Ok(new { message = "Consultation Has Already Expired" });
                }
                else if (res == 3)
                {
                    return Ok(new { message = "Consultation Has Already Been Canceled" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid Consultation Id" });
                }
            }
            else if (appointment != null)
            {
                var preconsultation = await _patientPreConsultation.GetPatientPreConsultation(appointment.PatientId);
                var patient = await _patient.GetPatientByIdAsync(appointment.PatientId);
                var patientEmail = patient.Patient.Email;

                var res = await _appointment.AdmitPatientOrSendPatientHome(Clerking);

                if (res == 0 && Clerking.IsAdmitted == true)
                {
                    if (Clerking.AdmissionNote == null)
                    {
                        return BadRequest(new { message = "Admission Must Have an Admission Note" });
                    }

                    appointment.IsCompleted = true;
                    appointment.IsAccepted = false;
                    appointment.IsCanceled = false;
                    appointment.IsCanceledByDoctor = false;
                    appointment.IsExpired = false;
                    appointment.IsPending = false;
                    appointment.IsRejected = false;
                    

                    var admissionToCreate = new Admission()
                    {
                        PatientId = appointment.PatientId,
                        DoctorId = Clerking.InitiatorId,
                        AdmissionNote = Clerking.AdmissionNote,
                        AppointmentId = Clerking.Id

                    };

                    await _admission.CreateAdmission(admissionToCreate);

                    var admissionInvoiceToCreate = new AdmissionInvoice()
                    {
                        Amount = admissionFee,
                        GeneratedBy = Clerking.InitiatorId,
                        AdmissionId = admissionToCreate.Id,
                    };


                    var admissionInvoiceId = await _admissionInvoice.CreateAdmissionInvoice(admissionInvoiceToCreate);

                    if (string.IsNullOrEmpty(admissionInvoiceId))
                    {
                        return BadRequest(new { response = "301", message = "Failed to generate invoice !!!, Try Again" });
                    }

                    var admissionRequestToCreate = new AdmissionServiceRequest()
                    {
                        
                        Amount = admissionFee,
                        AdmissionInvoiceId = admissionInvoiceId,
                    };

                    var result = await _admissionRequest.CreateAdmissionRequest(admissionRequestToCreate);

                    await _appointment.UpdateAppointment(appointment);
                    string emailSubject = "HMS Doctors Report";

                    string emailContent = "<p>" + clerking + " " + preconsultation + "</p>";
                    var message = new EmailMessage(new string[] { patientEmail }, emailSubject, emailContent);
                    _emailSender.SendEmail(message);


                    
                   

                    return Ok(new { message = "Patient Was Admitted" });
                }
                else if (res == 0 && Clerking.IsSentHome == true)
                {
                    appointment.IsCompleted = true;
                    appointment.IsAccepted = false;
                    appointment.IsCanceled = false;
                    appointment.IsCanceledByDoctor = false;
                    appointment.IsExpired = false;
                    appointment.IsPending = false;
                    appointment.IsRejected = false;

                    string emailSubject = "HMS Doctors Report";

                    string emailContent = "<p>" + clerking + " " + preconsultation + "</p>";
                    var message = new EmailMessage(new string[] { patientEmail }, emailSubject, emailContent);
                    _emailSender.SendEmail(message);


                    await _appointment.UpdateAppointment(appointment);
                    return Ok(new { message = "Patient Was Sent Home" });
                }
                else if (res == 1)
                {
                    return BadRequest(new { message = "Invalid Appointment Id" });
                }
                else if (res == 2)
                {
                    return Ok(new { message = "Appointment Has Already Been Rejected" });
                }
                else if (res == 3)
                {
                    return Ok(new { message = "Appontment Has Already Been Expired" });
                }
                else if (res == 4)
                {
                    return Ok(new { message = "Appontment Has Already Been Canceled" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid Appontment Id" });
                }
            }
            else
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

        }
    }
}
