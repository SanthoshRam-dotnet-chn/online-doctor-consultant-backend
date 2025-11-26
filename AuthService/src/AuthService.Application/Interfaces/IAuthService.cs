using AuthService.src.AuthService.Application.DTOs;

namespace AuthService.src.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
