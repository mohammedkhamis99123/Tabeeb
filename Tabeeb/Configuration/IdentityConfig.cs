using Microsoft.AspNetCore.Identity;
using Tabeeb.Domain.Entities;
using Tabeeb.Infrastructure.DbContexts;

namespace Tabeeb.Configuration
{
    public static class IdentityConfig
    {
        public static void ConfigIdentity(this IServiceCollection services)
        {
            services
    .AddIdentity<AppUser, AspNetRole>(options =>
    {
        // Password
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;

        // User
        options.User.RequireUniqueEmail = true;

        // Lockout
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // SignIn
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
        }
    }
}
