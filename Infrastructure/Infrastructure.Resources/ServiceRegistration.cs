using Application.Interfaces.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Resources
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureResources(this IServiceCollection services)
        {
            services.AddSingleton<IAppResource, SharedResources>();
        }
    }
}
