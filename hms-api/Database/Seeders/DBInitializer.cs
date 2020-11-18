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
            SeedUsers(userManager,db);
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


                    if (role == "Patient" || role == "patient")
                    {
                        var profile = new PatientProfile()
                        {
                            PatientId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}"
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
                }
            }

        }
    }
}
