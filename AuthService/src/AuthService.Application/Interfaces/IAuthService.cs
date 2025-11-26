using UserAuthService.src.AuthService.Application.DTOs;

namespace JwtTestingDemo.src.AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
