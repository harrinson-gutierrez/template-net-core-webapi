using Application.Enums;
using Newtonsoft.Json;

namespace Application.DTOs.Authentication
{
    public class AuthenticationLoginRequest
    {
        [JsonProperty("GrantType")]
        public GrantType? GrantType { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
