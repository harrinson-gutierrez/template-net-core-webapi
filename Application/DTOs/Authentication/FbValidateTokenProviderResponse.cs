using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.DTOs.Authentication
{
    public class FbValidateTokenProviderResponse
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public long DataAccessExpiredAt { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
