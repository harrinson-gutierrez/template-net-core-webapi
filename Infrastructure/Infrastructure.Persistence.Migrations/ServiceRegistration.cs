using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using FluentMigrator.Runner;

namespace Infrastructure.Persistence.Migrations
{
    public static class ServiceRegistration
    {
        public static IApplicationBuilder MigrateFM(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
            return app;
        }
    }
}