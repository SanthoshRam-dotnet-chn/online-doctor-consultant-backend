namespace AuthService.src.AuthService.Application.DTOs
{
    public class DoctorDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Specialization { get; set; }
        public int? Experience { get; set; }
    }
}
