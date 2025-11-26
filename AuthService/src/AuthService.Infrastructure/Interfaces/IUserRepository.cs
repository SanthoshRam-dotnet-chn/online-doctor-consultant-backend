using AuthService.src.AuthService.Domain.Entities;

namespace AuthService.src.AuthService.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
    }
}
