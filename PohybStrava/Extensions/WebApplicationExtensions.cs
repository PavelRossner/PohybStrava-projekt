using Microsoft.AspNetCore.Identity;
using PohybStrava.Models;

namespace PohybStrava.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task RegisterAdmin(this WebApplication webApplication, string userEmail, string userPassword)
        {
            var adminRoleName = "Admin";
            using (var scope = webApplication.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (!await roleManager.RoleExistsAsync(adminRoleName))
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));

                ApplicationUser user = await userManager.FindByEmailAsync(userEmail);

                if (user is null)
                    user = await CreateUser(userManager, userEmail, userPassword);

                if (!await userManager.IsInRoleAsync(user, adminRoleName))
                    await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }

        private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> userManager, string userEmail, string password)
        {
            ApplicationUser user = null;
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = userEmail, Email = userEmail }, password);
            if (result.Succeeded)
            {
                user = await userManager.FindByEmailAsync(userEmail);
            }

            return user;
        }
    }
}
