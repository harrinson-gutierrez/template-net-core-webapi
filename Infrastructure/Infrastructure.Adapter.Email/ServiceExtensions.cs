using Infrastructure.Adapter.Email.Interfaces;
using Infrastructure.Adapter.Email.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using Infrastructure.Adapter.Email.Settings;
using Amazon.Runtime;

namespace Infrastructure.Adapter.Email
{
    public static class ServiceExtensions
    {
        public static void AddEmailAdapter(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("AWS:Email"));
            services.AddSingleton<IEmailService, EmailAmazonAdapter>();
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSimpleEmailService>();
        }
    }
}
