using Dapper;
using Domain.Entities;
using Infrastructure.Identity.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<List<AppUserLogin>> GetAppUserLoginsByUser(int userId)
        {
            using IDbConnection conn = new NpgsqlConnection(ConnectionString);

            return (await conn.QueryAsync<AppUserLogin>("SELECT * FROM users_logins WHERE user_id = @userId", new { userId})).AsList();
        }

        public async Task<AppUserLogin> GetAppUserLoginByUserAndProvider(string loginProvider, string providerKey)
        {
            using IDbConnection conn = new NpgsqlConnection(ConnectionString);

            return await conn.QuerySingleOrDefaultAsync<AppUserLogin>(@"SELECT * FROM users_logins
                                                    WHERE login_provider = @loginProvider
                                                    AND provider_key = @providerKey", new { loginProvider, providerKey });
        }

        public async Task<List<AppUser>> GetUsers()
        {
            using IDbConnection conn = new NpgsqlConnection(ConnectionString);
            return (await conn.QueryAsync<AppUser>("SELECT * FROM users")).AsList();
        }

        public async Task<AppUserLogin> CreateAppUserLogin(AppUserLogin appUserLogin)
        {
            using IDbConnection conn = new NpgsqlConnection(ConnectionString);

            await conn.InsertAsync<string, AppUserLogin>(appUserLogin);

            return appUserLogin;
        }
    }
}
