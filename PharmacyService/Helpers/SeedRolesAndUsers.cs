using Microsoft.AspNetCore.Identity;
using PharmacyService.Authentication;
using PharmacyService.Contracts.Models;

namespace PharmacyService.Helpers
{
    public class SeedRolesAndUsers
    {
        public static async Task SeedRolesAndUsersAsync(IApplicationBuilder app, UserManager<ApplicationUser> userManager)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                // Initialize roles
                await authService.InitializeRolesAsync();

                // Initialize users
                await authService.InitializeUsersAsync(userManager);
            }
        }
    }
}
