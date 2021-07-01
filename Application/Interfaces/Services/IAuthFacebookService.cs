using Application.DTOs.Authentication;
using Application.Wrappers;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAuthFacebookService
    {
        Task<WrapperDataResponse<FbValidateTokenProviderResponse>> ValidateTokenAsync(string accessToken);

        Task<FbUserInfoResponse> GetUserInfoAsync(string accessToken);
    }
}
