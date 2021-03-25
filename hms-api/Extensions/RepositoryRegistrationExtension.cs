using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.Repositories;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Doctor.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HMS.Areas.Pharmacy.Repositories;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.Repositories;
using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Accountant.Repositories;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Repositories;
using HMS.Services.Interfaces;
using HMS.Services.Repositories;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Admissions.Repositories;
using HMS.Areas.Nurse.Repositories;
using HMS.Areas.Interfaces.Nurse;
using HMS.Areas.NHIS.Repositories;
using HMS.Areas.HealthInsurance.Repositories;
using HMS.Areas.HealthInsurance.Interfaces;

namespace HMS.Extensions
{
    public static class RepositoryRegistrationExtension
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            IConfiguration config;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                config = serviceProvider.GetService<IConfiguration>();
            }

            services.AddTransient<IAccountantProfile, AccountantProfileRepository>();
            services.AddTransient<IAccountantInvoice, AccountantInvoiceRepository>();

            /*----Adding of admin repo*/

            services.AddTransient<IHealthPlan, HealthPlanRepository>();
            services.AddTransient<IAdminProfile, AdminProfileRepository>();
            services.AddTransient<IAppointment, AppointmentRepository>();
            services.AddTransient<IServiceCategory, ServiceCategoryRepository>();
            services.AddTransient<IServices, ServicesRepository>();
            services.AddTransient<IDoctor, DoctorRepository>();
            
            services.AddTransient<IAccount, AccountRepository>();
            services.AddTransient<IRegister, RegisterRepository>();
            services.AddTransient<IConsultation, ConsultationRepository>();
            services.AddTransient<IRegistrationInvoice, RegistrationInvoiceRepository>();
            services.AddTransient<IServiceRequest, Areas.Admin.Repositories.ServiceRequestRepository>();
            services.AddTransient<IServiceRequestInvoice, ServiceRequestInvoiceRepository>();

            /*----Adding of admission repo*/

            services.AddTransient<IAdmission, AdmissionRepository>();
            services.AddTransient<IAdmissionInvoice, InvoiceRepository>();
            services.AddTransient<IAdmissionServiceRequest, Areas.Admissions.Repositories.ServiceRequestRepository>();
            services.AddTransient<IAdmissionDrugDispensing, DrugDispensingRepository>();
            services.AddTransient<IBed, BedRepository>();
            services.AddTransient<IWard, WardRepository>();
            services.AddTransient<IAdmissionNote, AdmissionNoteRepository>();
            services.AddTransient<IObservationChart, ObservationChartRepository>();
            services.AddTransient<IPrescription, PrescriptionRepository>();
            services.AddTransient<IMedication, MedicationRepository>();
            services.AddTransient<IWardPersonnel, WardPersonnelRepository>();



            /*----Adding of Doctor repo*/
            services.AddTransient<IDoctorProfile, DoctorProfileRepository>();
            services.AddTransient<IDoctorAppointment, DoctorAppointmentRepository>();
            services.AddTransient<IDoctorConsultation, DoctorConsultationRepository>();
            services.AddTransient<IDoctorClerking, DoctorClerkingRepository>();
            services.AddTransient<ISurgery, SurgeryRepository>();
            services.AddTransient<IMyPatient, MyPatientRepository>();

            //Adding Health Insurance Repo
            services.AddTransient<IHMO, HMORepository>();
            services.AddTransient<IHMOHealthPlan, HMOHealthPlanRepository>();
            services.AddTransient<IHMOHealthPlanDrugPrice, HMOHealthPlanDrugPriceRepository>();
            services.AddTransient<IHMOHealthPlanServicePrice, HMOHealthPlanServicePriceRepository>();
            services.AddTransient<IHMOHealthPlanPatient, HMOHealthPlanPatientRepository>();
            services.AddTransient<IHMOUserGroup, HMOUserGroupRepository>();
            services.AddTransient<IHMOSubUserGroup, HMOSubUserGroupRepository>();
            services.AddTransient<IHMOSubUserGroupPatient, HMOSubUserGroupPatientRepository>();
            services.AddTransient<INHISHealthPlan, NHISHealthPlanRepository>();
            services.AddTransient<INHISHealthPlanPatient, NHISHealthPlanPatientRepository>();
            services.AddTransient<INHISHealthPlanService, NHISHealthPlanServiceRepository>();
            services.AddTransient<INHISHealthPlanDrug, NHISHealthPlanDrugRepository>();
            services.AddTransient<IHMOAdmin, HMOAdminRepository>();



            //Adding Lab Repo
            services.AddTransient<ILabProfile, LabProfileRepository>();

            /*----Adding of nurse repo*/
            services.AddTransient<INurse, NurseRepository>();
           

            //Adding patient repo
            services.AddTransient<IPatientProfile, PatientProfileRepository>();
            services.AddTransient<IPatientAppointment, PatientAppointmentRepository>();
            services.AddTransient<IPatientPreConsultation, PatientPreConsultationRepository>();
            services.AddTransient<IPatientConsultation, PatientConsultationRepository>();
          

            /*----Adding of pharmacy repo*/
            services.AddTransient<IPharmacyProfile, PharmacyProfileRepository>();
            services.AddTransient<IDrug, DrugRepository>();
            services.AddTransient<IDrugPrice, DrugPriceRepository>();
            services.AddTransient<IDrugCosting, DrugCostingRepository>();
            services.AddTransient<IDrugInvoicing, DrugInvoicingRepository>();
            services.AddTransient<IDrugBatch, DrugBatchRepository>();



            /*----Adding of transaction repo*/
            services.AddTransient<ITransactionLog, TransactionLogRepository>();
            services.AddTransient<IReports, ReportsRepository>();

            /* --- Adding common Repo */
            services.AddTransient<IUser, UserRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
