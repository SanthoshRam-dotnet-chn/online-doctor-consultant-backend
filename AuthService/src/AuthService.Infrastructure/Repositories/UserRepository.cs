using JwtTestingDemo.src.AuthService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserAuthService.src.AuthService.Domain.Entities;
using UserAuthService.src.AuthService.Infrastructure.Data;

namespace UserAuthService.src.AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
    }
