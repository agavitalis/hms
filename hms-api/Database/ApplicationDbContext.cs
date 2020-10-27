
using HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HMS.Areas.Admin.Models;
using HMS.Areas.Lab.Models;
using HMS.Areas.Patient.Models;
using HMS.Areas.Pharmacy.Models;
using HMS.Areas.Doctor.Models;
using HMS.Areas.Account.Models;

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

        /*----------register Account models here---------*/
        public DbSet<HealthPlan> HealthPlans { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        /*----------register Doctor models here---------*/
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        
        public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
    

        /*----------register Lab models here---------*/
        public DbSet<LabTestCategory> LabTestCategories { get; set; }
        public DbSet<LabTestInLabTestCategory> LabTestInLabTestCategories { get; set; }
        public DbSet<LabTestInLabTestSubCategory> LabTestInLabTestSubCategories { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestSubCategory> LabTestSubCategories { get; set; }
        public DbSet<LabProfile> LabProfiles { get; set; }

        /*----------register patient models here---------*/

        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<PatientDrugPrescription> PatientDrugPrescritions { get; set; }
        public DbSet<PatientLabPrescription> PatientLabPrescritions { get; set; }
        public DbSet<PatientPreConsultation> PatientPreConsultation { get; set; }
        public DbSet<PatientQueue> PatientQueue { get; set; }

        /*----------register the pharmacy models here---------*/
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugCategory> DrugCategories { get; set; }
        public DbSet<DrugSubCategory> DrugSubCategories { get; set; }
        public DbSet<DrugInDrugCategory> DrugInDrugCategories { get; set; }
        public DbSet<DrugInDrugSubCategory> DrugInDrugSubCategories { get; set; }
        public DbSet<PharmacyProfile> PharmacyProfiles { get; set; }


    }
}
