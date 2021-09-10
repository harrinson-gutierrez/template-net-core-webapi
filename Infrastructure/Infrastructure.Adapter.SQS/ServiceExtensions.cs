using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Adapter.Email.Settings;
using Amazon.Runtime;
using Amazon.SQS;
using Infrastructure.Adapter.SQS.Interfaces;
using Infrastructure.Adapter.SQS.Adapters;

namespace Infrastructure.Adapter.SQS
{
    public static class ServiceExtensions
    {
        public static void AddSQSAdapter(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqsOptions>(configuration.GetSection("AWS:SQS"));
            services.AddSingleton<ISqsService, SqsAmazonAdapter>();
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSQS>();
        }
    }
}
