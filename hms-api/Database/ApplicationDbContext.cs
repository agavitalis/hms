using HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /*----------register the models here---------*/
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /*----------register Admission models here---------*/
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionInvoice> AdmissionInvoices { get; set; }
        public DbSet<AdmissionRequest> AdmissionRequests { get; set; }
        public DbSet<AdmissionPrescription> AdmissionPrescriptions { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<ObservationChart> ObservationCharts { get; set; }
        public DbSet<AdmissionMedication> AdmissionMedications { get; set; }
        public DbSet<AdmissionMedicationDispensing> AdmissionMedicationDispensings { get; set; }

        /*----------register Account models here---------*/
        public DbSet<HealthPlan> HealthPlans { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountInvoice> AccountInvoices { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceInvoice> ServiceInvoices { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceRequestResult> ServiceRequestResults { get; set; }
        public DbSet<ServiceRequestResultImage> ServiceRequestResultImages { get; set; }

        public DbSet<AccountantProfile> AccountantProfiles { get; set; }

        /*----------register Admin models here---------*/
        public DbSet<AdminProfile> AdminProfiles { get; set; }

        /*----------register Doctor models here---------*/
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<DoctorOfficeTime> DoctorOfficeTimes { get; set; }
        public DbSet<DoctorEducation> DoctorEducations { get; set; }
        public DbSet<DoctorExperience> DoctorExperiences { get; set; }
        public DbSet<DoctorSocial> DoctorSocials { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Appointment> DoctorAppointments { get; set; }
        public DbSet<MyPatient> MyPatients { get; set; }

        public DbSet<DoctorClerking> DoctorClerkings { get; set; }

       
        public DbSet<LabProfile> LabProfiles { get; set; }

        /*----------register patient models here---------*/

        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<PatientPreConsultation> PatientPreConsultation { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<RegistrationInvoice> RegistrationInvoices { get; set; }

        /*----------register the pharmacy models here---------*/
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugPrice> DrugPrices { get; set; }
        public DbSet<DrugDispensing> DrugDispensings { get; set; }
        public DbSet<DrugDispensingInvoice> DrugDispensingInvoices { get; set; }

   
        public DbSet<PharmacyProfile> PharmacyProfiles { get; set; }

        /*----------register the pharmacy models here---------*/
        public DbSet<Transactions> Transactions  { get; set; }

        /*----------register the ward models here---------*/
        public DbSet<Ward> Wards { get; set; }
    }
}
