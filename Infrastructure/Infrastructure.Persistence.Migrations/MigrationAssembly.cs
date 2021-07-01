using System;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Migrations
{
    public class MigrationAssembly
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        // <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString( "Host=localhost;Username=postgres;Password=123456;Database=ada2;")
                    .ScanIn(typeof(MigrationAssembly).Assembly).For.Migrations().
                                                                For.EmbeddedResources())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddSingleton<IAssemblySourceItem>(new AssemblySourceItem(Assembly.GetExecutingAssembly()))
                //.AddScoped<PostgresQuoter, CustomPostgresQuoter>()
                .BuildServiceProvider(false);
        }

        /// <summary> 
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
