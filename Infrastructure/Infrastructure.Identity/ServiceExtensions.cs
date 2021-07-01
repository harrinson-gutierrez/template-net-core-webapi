using Domain.Entities;
using Domain.Settings;
using Infrastructure.Identity.Stores;
using Infrastructure.Identity.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomJwtOptions>(options => configuration.GetSection("Authentication:Jwt").Bind(options));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

            }).AddRoleStore<RoleStoreRepository>()
              .AddUserStore<UserStoreRepository>()
               .AddDefaultTokenProviders()
               .AddErrorDescriber<CustomIdentityErrorDescriber>();

            var customJwtOptions = configuration.GetSection("Authentication:Jwt").Get<CustomJwtOptions>();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = customJwtOptions.Issuer,
                ValidAudience = customJwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customJwtOptions.Key))
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(cfg => cfg.SlidingExpiration = true)
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
