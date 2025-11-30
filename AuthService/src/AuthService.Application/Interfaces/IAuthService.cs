using AuthService.src.AuthService.Application.DTOs;

namespace AuthService.src.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request); 
        Task<UserDto?> GetUserByEmailAsync(string email);

        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(Guid id);
        Task<PatientDto?> GetPatientByIdAsync(Guid id);


    }
}
