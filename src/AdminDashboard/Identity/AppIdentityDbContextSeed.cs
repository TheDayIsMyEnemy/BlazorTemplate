using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task Seed(
            AppIdentityDbContext appIdentityDbContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger logger)
        {

            if (appIdentityDbContext.Database.IsMySql())
                await appIdentityDbContext.Database.MigrateAsync();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new Role(Constants.AdminRole));

                logger.LogInformation("Default administrator role created");
            }

            if (!userManager.Users.Any())
            {
                await CreateUser(
                    Constants.DefaultAdminEmail,
                    Constants.DefaultAdminPassword,
                    Constants.AdminRole,
                    userManager
                );

                logger.LogInformation("Default user created");
            }
        }

        private static async Task<User> CreateUser(
        string email,
        string password,
        string role,
        UserManager<User> userManager
    )
        {
            var user = new User { UserName = email, Email = email, };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);

            return user;
        }
    }
}