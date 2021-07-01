using System;

namespace Application.DTOs.Authentication
{
    public class AuthenticationResponse
    {
        public string Type { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Expiration { get; set; }
    }
}
