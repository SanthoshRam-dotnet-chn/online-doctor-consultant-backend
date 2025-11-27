using AuthService.src.AuthService.Application.DTOs;

namespace AuthService.src.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(Guid id);


    }
}
