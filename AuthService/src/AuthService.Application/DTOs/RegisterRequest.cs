namespace UserAuthService.src.AuthService.Application.DTOs
{
    public class RegisterRequest
    {
        public string Role { get; set; } = "patient";
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
