using Newtonsoft.Json;

namespace Application.DTOs.Authentication
{
    public class FbUserInfoResponse
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set;  }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
