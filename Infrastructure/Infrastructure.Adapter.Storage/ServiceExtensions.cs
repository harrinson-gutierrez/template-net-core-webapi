using Amazon.Runtime;
using Amazon.S3;
using Infrastructure.Adapter.Storage.Adapters;
using Infrastructure.Adapter.Storage.Interfaces;
using Infrastructure.Adapter.Storage.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Adapter.Storage
{
    public static class ServiceExtensions
    {
        public static void AddStorageAdapter(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StorageOptions>(configuration.GetSection("AWS:Storage"));
            services.AddSingleton<IStorageService, StorageAmazonAdapter>();
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }
    }
}
