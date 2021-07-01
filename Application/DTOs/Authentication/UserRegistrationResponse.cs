using System.Collections.Generic;

namespace Application.DTOs.Authentication
{
    public class UserRegistrationResponse
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}
