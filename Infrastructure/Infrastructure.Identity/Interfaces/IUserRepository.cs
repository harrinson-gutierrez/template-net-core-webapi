using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetUsers();
        Task<List<AppUserLogin>> GetAppUserLoginsByUser(int userId);
        Task<AppUserLogin> GetAppUserLoginByUserAndProvider(string loginProvider, string providerKey);
        Task<AppUserLogin> CreateAppUserLogin(AppUserLogin appUserLogin);
    }
}
