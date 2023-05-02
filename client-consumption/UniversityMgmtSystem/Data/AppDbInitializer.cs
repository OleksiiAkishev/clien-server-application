using Microsoft.AspNetCore.Identity;
using UniversityMgmtSystem.Data.Static;
using UniversityMgmtSystem.Models;

namespace UniversityMgmtSystem.Data
{
    public class AppDbInitializer
    {
        /*public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Users.Any()) 
                {
                    new ApplicationUser()
                    {
                        FullName = "User 1"
                    };
                    context.SaveChanges();
                }
            }
        }*/

        public static async Task SeedUsersAndRoles(IApplicationBuilder applicationBuilder) 
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                }

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminUser = await userManager.FindByEmailAsync("admin@email.com");
                if (adminUser == null) 
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin",
                        UserName = "admin",
                        Email = "admin@email.com"
                    };
                    await userManager.CreateAsync(newAdminUser, "qwertY!12345");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                var studentUser = await userManager.FindByEmailAsync("student@email.com");
                if (studentUser == null)
                {
                    var newStudentUser = new ApplicationUser()
                    {
                        FullName = "Student",
                        UserName = "student",
                        Email = "student@email.com"
                    };
                    await userManager.CreateAsync(newStudentUser, "studenT?9876");
                    await userManager.AddToRoleAsync(newStudentUser, UserRoles.Student);
                }

                var teacherUser = await userManager.FindByEmailAsync("teacher@email.com");
                if (teacherUser == null)
                {
                    var newTeacherUser = new ApplicationUser()
                    {
                        FullName = "Teacher",
                        UserName = "teacher",
                        Email = "teacher@email.com"
                    };
                    await userManager.CreateAsync(newTeacherUser, "teacheR?543");
                    await userManager.AddToRoleAsync(newTeacherUser, UserRoles.Teacher);
                }
            }
        }
    }
}
