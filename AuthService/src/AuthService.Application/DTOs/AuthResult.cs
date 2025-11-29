namespace AuthService.src.AuthService.Application.DTOs
{
    public class AuthResult
    {
        public AuthResponse User { get; set; }
        public string Token { get; set; }
    }
}
