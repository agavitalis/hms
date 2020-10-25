using HMS.Areas.Lab.Interfaces;
using HMS.Areas.Lab.Repositories;
using HMS.Services.Interfaces.Account;
using HMS.Services.Interfaces.Admin;
using HMS.Services.Interfaces.Doctor;
using HMS.Services.Interfaces.Patient;
using HMS.Services.Interfaces.Pharmacy;
using HMS.Services.Repositories.Account;
using HMS.Services.Repositories.Admin;
using HMS.Services.Repositories.Doctor;
using HMS.Services.Repositories.Patient;
using HMS.Services.Repositories.Pharmacy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            services.AddTransient<IAccountProfile, AccountProfileRepository>();
            services.AddTransient<IAccountInvoice, AccountInvoiceRepository>();

            /*----Adding of Doctor repo*/
            services.AddTransient<IDoctorProfile, DoctorProfileRepository>();
            services.AddTransient<IDoctorSpecialization, DoctorSpecializationRepository>();
            services.AddTransient<IDoctor, DoctorRepository>();

            //Adding Lab Repo
            services.AddTransient<ILabProfile, LabProfileRepository>();
            services.AddTransient<ILabTest, LabTestRepository>();
            services.AddTransient<ILabTestCategory, LabTestCategoryRepository>();
            services.AddTransient<ILabTestSubCategory, LabTestSubCategoryRepository>();
            services.AddTransient<ILabTestInLabTestCategory, LabTestInLabTestCategoryRepository>();
            services.AddTransient<ILabTestInLabTestSubCategory, LabTestInLabTestSubCategoryRepository>();

            /*----Adding of pharmacy repo*/
            services.AddTransient<IPharmacyProfile, PharmacyProfileRepository>();
            services.AddTransient<IDrug, DrugRepository>();
            services.AddTransient<IDrugCategory, DrugCategoryRepository>();
            services.AddTransient<IDrugSubCategory, DrugSubCategoryRepository>();
            services.AddTransient<IDrugInDrugCategory, DrugInDrugCategoryRepository>();
            services.AddTransient<IDrugInDrugSubCategory, DrugInDrugSubCategoryRepository>();

            //Adding patient repo
            services.AddTransient<IPatientProfile, PatientProfileRepository>();
            services.AddTransient<IPatientPreConsultation, PatientPreConsultationRepository>();
            services.AddTransient<IPatientQueue, PatientQueueRepository>();
            services.AddTransient<IPatientPrescription, PatientPresciptionRepository>();

            //Admin
            services.AddTransient<IAdmin, AdminRepository>();

        }
    } 
}
