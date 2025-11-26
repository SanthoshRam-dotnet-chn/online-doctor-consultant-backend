namespace UserAuthService.src.AuthService.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = "patient";

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
