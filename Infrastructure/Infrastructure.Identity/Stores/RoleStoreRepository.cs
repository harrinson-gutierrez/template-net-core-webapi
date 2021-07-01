using Dapper;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Stores
{
    public class RoleStoreRepository : BaseRepository, IRoleStore<AppRole>
    {
        public RoleStoreRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);
            await con.InsertAsync<Guid, AppRole>(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);
            await con.DeleteAsync(role);
            return IdentityResult.Success;
        }

        public async Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);
            return await con.QuerySingleOrDefaultAsync<AppRole>(@"SELECT * FROM roles
                                                           WHERE role_id = @roleId", new { roleId });
        }

        public async Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);
            return await con.QuerySingleOrDefaultAsync<AppRole>(@"SELECT * FROM roles
                                                           WHERE role_name = @normalizedUserName", new { normalizedRoleName });
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.role_name);
        }

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.role_id.ToString());
        }

        public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.role_name);
        }

        public Task SetNormalizedRoleNameAsync(AppRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.role_name = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            role.role_name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);
            await con.UpdateAsync(role);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }
    }
}
