using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<Guid, RefreshToken>
    {
        Task<RefreshToken> GetByJwtId(Guid jwtId);
    }
}
