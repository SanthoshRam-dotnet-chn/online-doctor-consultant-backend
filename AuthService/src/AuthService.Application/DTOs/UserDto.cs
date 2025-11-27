namespace AuthService.src.AuthService.Application.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

   
    }
}
