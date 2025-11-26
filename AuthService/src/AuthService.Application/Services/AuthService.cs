using JwtTestingDemo.src.AuthService.Application.Interfaces;
using JwtTestingDemo.src.AuthService.Infrastructure.Interfaces;
using JwtTestingDemo.src.AuthService.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using UserAuthService.src.AuthService.Application.DTOs;
using UserAuthService.src.AuthService.Domain.Entities;

namespace JwtTestingDemo.src.AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly JwtTokenGenerator _jwt;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(IUserRepository repo, JwtTokenGenerator jwt)
        {
            _repo = repo;
            _jwt = jwt;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
        {
            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null) throw new Exception("Email already exists");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Role = req.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, req.Password);

            await _repo.AddAsync(user);

            return new AuthResponse
            {
                Email = user.Email,
                Role = user.Role,
                Token = _jwt.GenerateToken(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest req)
        {
            var user = await _repo.GetByEmailAsync(req.Email)
                ?? throw new Exception("Invalid email or password");

            var verification = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if (verification == PasswordVerificationResult.Failed)
                throw new Exception("Invalid email or password");

            return new AuthResponse
            {
                Email = user.Email,
                Role = user.Role,
                Token = _jwt.GenerateToken(user)
            };
        }
    }
}