﻿using HMS.Areas.Lab.Interfaces;
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

            /*----Adding of admin repo*/

            services.AddTransient<IHealthPlan, HealthPlanRepository>();
            services.AddTransient<IAppointment, AppointmentRepository>();
            services.AddTransient<IServices, ServicesRepository>();
            services.AddTransient<IDoctor, DoctorRepository>();
            services.AddTransient<IWard, WardRepository>();
            services.AddTransient<IAccount, AccountRepository>();
            services.AddTransient<IRegister, RegisterRepository>();


            /*----Adding of Doctor repo*/
            services.AddTransient<IDoctorProfile, DoctorProfileRepository>();
            services.AddTransient<IDoctorAppointment, DoctorAppointmentRepository>();

            //Adding Lab Repo
            services.AddTransient<ILabProfile, LabProfileRepository>();
            services.AddTransient<ILabTest, LabTestRepository>();
            services.AddTransient<ILabTestCategory, LabTestCategoryRepository>();
            services.AddTransient<ILabTestSubCategory, LabTestSubCategoryRepository>();
            services.AddTransient<ILabTestInLabTestCategory, LabTestInLabTestCategoryRepository>();
            services.AddTransient<ILabTestInLabTestSubCategory, LabTestInLabTestSubCategoryRepository>();

            //Adding patient repo
            services.AddTransient<IPatientProfile, PatientProfileRepository>();
            services.AddTransient<IPatientPreConsultation, PatientPreConsultationRepository>();
            services.AddTransient<IPatientConsultation, PatientQueueRepository>();
            services.AddTransient<IPatientPrescription, PatientPresciptionRepository>();

            /*----Adding of pharmacy repo*/
            services.AddTransient<IPharmacyProfile, PharmacyProfileRepository>();
            services.AddTransient<IDrug, DrugRepository>();
            services.AddTransient<IDrugCategory, DrugCategoryRepository>();
            services.AddTransient<IDrugSubCategory, DrugSubCategoryRepository>();
            services.AddTransient<IDrugInDrugCategory, DrugInDrugCategoryRepository>();
            services.AddTransient<IDrugInDrugSubCategory, DrugInDrugSubCategoryRepository>();

            /* --- Adding common Repo */
            services.AddTransient<IUser, UserRepository>();
        }
    }
}
