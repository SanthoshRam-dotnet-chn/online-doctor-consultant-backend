using System.Numerics;
using AuthService.src.AuthService.Application.DTOs;
using AuthService.src.AuthService.Domain.Entities;
using AuthService.src.AuthService.Infrastructure.Data;
using AuthService.src.AuthService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.src.AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _db.Users.Where(u => u.Role.ToLower() == role.ToLower()).ToListAsync();
        }

        public async Task<User?> GetDoctorByIdAsync(Guid id)
        {
            return await _db.Users.FirstOrDefaultAsync(d => d.UserId == id);
        }

        public async Task<User?> GetPatientByIdAsync(Guid id)
        {
            return await _db.Users.FirstOrDefaultAsync(p =>
                p.UserId == id && p.Role.ToLower() == "patient"
            );
        }
        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

    }
}
