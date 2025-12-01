using AuthService.src.AuthService.Application.DTOs;
using AuthService.src.AuthService.Domain.Entities;
using System.Numerics;

namespace AuthService.src.AuthService.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();            
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<User?> GetDoctorByIdAsync(Guid id);
        Task<User?> GetPatientByIdAsync(Guid id);


    }
}
