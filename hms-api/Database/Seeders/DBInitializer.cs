using HMS.Database;
using HMS.Models;
using HMS.Services.Enumerators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Auth.Database.Seeders
{
    public static class DBInitializer 
    {
        
        public static void InitializeDB(this IApplicationBuilder app)
        {

            var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            RunMigration(db);
            SeedRoles(roleManager);
           // string healthPlanId = SeedHealthPlan(db);
           // string accountId = SeedAccount(db, healthPlanId);
            SeedUsers(userManager, db);
        }

        public static void RunMigration(ApplicationDbContext db)
        {
            
            if (db.Database.GetPendingMigrations().Count() > 0)
            {
                db.Database.Migrate();
            }

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    IdentityRole Role = new IdentityRole();
                    Role.Name = role;
                    IdentityResult roleResult = roleManager.CreateAsync(Role).Result;
                }
            }
        }

        public static string SeedHealthPlan(ApplicationDbContext db)
        {
            HealthPlan healthPlan = new HealthPlan()
            {
                Name = "Default",
                Cost = 1000,
                Renewal = 1000,
                NoOfPatients = 1, 
                NoOfAccounts = 1,
                InstantBilling = true,
                CreatedBy = "Ugochukwu"
            };

            db.HealthPlans.Add(healthPlan);
            db.SaveChangesAsync().Wait();
            return healthPlan.Id;
        }

        public static string SeedAccount(ApplicationDbContext db, string HealthPlanId)
        {
            Account account = new Account()
            {
                Name = "Default",
                PhoneNumber = "123456789",
                HealthPlanId = HealthPlanId,
                CreatedBy = "Ugochukwu"
            };

            db.Accounts.Add(account);
            db.SaveChangesAsync().Wait();
            return account.Id;
        }


        public static void SeedUsers(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            { 
                var username = role + "@hms.com";
                var first_name = char.ToUpper(role[0]) + role.Substring(1);
                var last_name = "Hms";
                var password = first_name + "1@hms.com";

                if (userManager.FindByNameAsync(username).Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = username;
                    user.Email = username;
                    user.FirstName = first_name;
                    user.LastName = last_name;
                    user.UserType = role;

                    IdentityResult result = userManager.CreateAsync(user, password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, role).Wait();

                    }

                    if (role == "Admin" || role == "admin")
                    {
                        var profile = new AdminProfile()
                        {
                            AdminId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"                            
                        };
                        db.AdminProfiles.Add(profile);
                        db.SaveChangesAsync().Wait();
                    }

                    if (role == "Patient" || role == "patient")
                    {
                        var profile = new PatientProfile()
                        {
                            PatientId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            //AccountId = AccountId
                        };
                        db.PatientProfiles.Add(profile);
                        db.SaveChangesAsync().Wait();
                    }

                    if (role == "Doctor" || role == "doctor")
                    {
                        var profile = new DoctorProfile()
                        {
                            DoctorId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"
                        };
                        db.DoctorProfiles.Add(profile);
                        db.SaveChangesAsync().Wait();
                    }

                    if (role == "Accountant" || role == "accountant")
                    {
                        var profile = new AccountantProfile()
                        {
                            AccountantId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"
                        };
                        db.AccountantProfiles.Add(profile);
                       db.SaveChangesAsync().Wait();
                    }

                    if (role == "Pharmacy" || role == "pharmacy")
                    {
                        var profile = new PharmacyProfile()
                        {
                            PharmacyId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"
                        };
                        db.PharmacyProfiles.Add(profile);
                        db.SaveChangesAsync().Wait();
                    }

                    if (role == "Lab" || role == "lab")
                    {
                        var profile = new LabProfile()
                        {
                            LabId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"
                        };
                        db.LabProfiles.Add(profile);
                        db.SaveChangesAsync().Wait();
                    }
                }
            }

        }
    }
}
