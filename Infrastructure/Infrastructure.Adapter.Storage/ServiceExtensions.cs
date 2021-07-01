using Amazon.S3;
using Domain.Settings;
using Infrastructure.Adapter.Storage.Adapters;
using Infrastructure.Adapter.Storage.Interfaces;
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
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
        }
    }
}
