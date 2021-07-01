using FluentMigrator.Runner;
using Infrastructure.Persistence.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Migrations
{
    public static class ServiceExtensions
    {
        public static void AddFluentMigratorConsole(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c
                    .AddPostgres()
                    .WithGlobalConnectionString(configuration.GetConnectionString("Default"))
                    .ScanIn(typeof(MigrationAssembly).Assembly).For.All());
        }
    }
}
