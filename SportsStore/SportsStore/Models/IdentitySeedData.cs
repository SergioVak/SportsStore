using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string ADMIN_USER = "Admin";
        private const string ADMIN_PASSWORD = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app.ApplicationServices
                .GetRequiredService < UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(ADMIN_USER);

            if(user == null)
            {
                user = new IdentityUser(ADMIN_USER);
                await userManager.CreateAsync(user, ADMIN_PASSWORD);
            }
        }
    }
}