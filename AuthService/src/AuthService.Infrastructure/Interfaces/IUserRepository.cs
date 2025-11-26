using UserAuthService.src.AuthService.Domain.Entities;

namespace JwtTestingDemo.src.AuthService.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
    }
}
