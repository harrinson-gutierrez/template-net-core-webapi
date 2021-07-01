using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IConfiguration Configuration;

        public BaseRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected string ConnectionString
        {
            get { return Configuration.GetConnectionString("Default"); }
        }
    }
}
