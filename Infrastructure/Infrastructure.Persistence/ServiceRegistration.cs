using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Repositories;
using Application.Interfaces.Repositories;
using Dapper;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
            #region Repositories
            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryPostgresql<,>));
            #endregion
        }
    }
}