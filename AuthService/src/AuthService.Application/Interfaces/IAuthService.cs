using AuthService.src.AuthService.Application.DTOs;

namespace AuthService.src.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(Guid id);


    }
}
