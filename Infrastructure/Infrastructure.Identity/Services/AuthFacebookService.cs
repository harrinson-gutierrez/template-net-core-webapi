using Application.DTOs.Authentication;
using Application.Interfaces.Services;
using Application.Wrappers;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AuthFacebookService : IAuthFacebookService
    {
        private readonly IOptions<FacebookAuthOptions> FacebookAuthConfiguration;
        private readonly IHttpClientFactory HttpClientFactory;

        public AuthFacebookService(IOptions<FacebookAuthOptions> facebookAuthConfiguration, IHttpClientFactory httpClientFactory)
        {
            FacebookAuthConfiguration = facebookAuthConfiguration;
            HttpClientFactory = httpClientFactory;
        }
        public async Task<WrapperDataResponse<FbValidateTokenProviderResponse>> ValidateTokenAsync(string accessToken)
        {
            var formatUrl = FacebookAuthConfiguration.Value.ApiUrl + string.Format(FacebookAuthConfiguration.Value.TokenValidationUrl, 
                                                accessToken, FacebookAuthConfiguration.Value.AppId, FacebookAuthConfiguration.Value.AppSecret);
            var result = await HttpClientFactory.CreateClient().GetAsync(formatUrl);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WrapperDataResponse<FbValidateTokenProviderResponse>>(response);
        }

        public async Task<FbUserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            var formatUrl = FacebookAuthConfiguration.Value.ApiUrl + string.Format(FacebookAuthConfiguration.Value.GetUserInfoUrl, accessToken);
            var result = await HttpClientFactory.CreateClient().GetAsync(formatUrl);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<FbUserInfoResponse>(response);
        }
    }
}
