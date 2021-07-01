using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Identity.Interfaces;
using Infrastructure.Identity.Repositories;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:Configuration"));
            services.Configure<FacebookAuthOptions>(configuration.GetSection("Authentication:Facebook"));
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthFacebookService, AuthFacebookService>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
