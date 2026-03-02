using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SIWES360.Domain.Entities.Departments;
using SIWES360.Domain.Entities.User;
using SIWES360.Domain.Enums;

namespace SIWES360.Infrastructure.Persistence
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            await db.Database.MigrateAsync();
            await SeedDepartmentsWithAdmins(db, userManager);
        }

        private static async Task SeedDepartmentsWithAdmins(
            ApplicationDbContext db,
            UserManager<User> userManager)
        {
            if (db.Departments.Any())
                return;

            var departments = new List<(string Name, string AdminEmail)>
        {
            ("Biomedical Engineering", "biomedical-admin@school.com"),
            ("Computer Engineering", "computereng-admin@school.com"),
            ("Civil Engineering", "civil-admin@school.com"),
            ("Electrical Engineering", "electrical-admin@school.com"),
            ("Mechanical Engineering", "mechanical-admin@school.com"),
            ("Mechatronics Engineering", "mechatronics-admin@school.com"),
            ("Telecommunications Engineering", "telecom-admin@school.com"),
            ("Computer Science", "cs-admin@school.com"),
            ("Information Technology", "it-admin@school.com")
        };

            foreach (var (Name, AdminEmail) in departments)
            {
                var admin = await userManager.FindByEmailAsync(AdminEmail);

                if (admin == null)
                {
                    admin = new User
                    {
                        UserName = AdminEmail,
                        Email = AdminEmail,
                        EmailConfirmed = true,
                        Role = UserRole.Administrator
                    };

                    await userManager.CreateAsync(admin, "Admin@123");
                }

                var department = new Department
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    AdminUserId = admin.Id,
                };

                db.Departments.Add(department);
            }

            await db.SaveChangesAsync();
        }
    }
}