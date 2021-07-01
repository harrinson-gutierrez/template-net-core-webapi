using Dapper;
using Domain.Entities;
using Infrastructure.Identity.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Repositories
{
    public class RefreshTokenRepository : RepositoryPostgresql<Guid, RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<RefreshToken> GetByJwtId(Guid jwtId)
        {
            using IDbConnection conn = new NpgsqlConnection(ConnectionString);
            return await conn.QuerySingleOrDefaultAsync<RefreshToken>(@"SELECT * FROM refresh_tokens
                                                           WHERE jwt_id = @jwtId", new { jwtId = jwtId.ToString() });
        }
    }
}
