namespace UserAuthService.src.AuthService.Application.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
