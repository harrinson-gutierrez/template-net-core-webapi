namespace Application.DTOs.Authentication
{
    public class ConfirmEmailConfirmationRequest
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
