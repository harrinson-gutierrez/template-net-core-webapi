namespace Application.DTOs.Authentication
{
    public class RecoveryPasswordConfirmRequest
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }
    }
}
