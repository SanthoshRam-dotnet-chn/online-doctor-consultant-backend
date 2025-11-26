using AuthService.src.AuthService.Application.DTOs;
using AuthService.src.AuthService.Application.Exceptions;
using AuthService.src.AuthService.Application.Interfaces;
using AuthService.src.AuthService.Domain.Entities;
using AuthService.src.AuthService.Infrastructure.Interfaces;
using AuthService.src.AuthService.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;

namespace AuthService.src.AuthService.Application.Services
{
    public class UserAuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly JwtTokenGenerator _jwt;
        private readonly PasswordHasher<User> _hasher;

        public UserAuthService(IUserRepository repo, JwtTokenGenerator jwt)
        {
            _repo = repo;
            _jwt = jwt;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
        {
            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null) throw new EmailAlreadyExistsException();


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
                ?? throw new InvalidCredentialsException();


            var verification = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if (verification == PasswordVerificationResult.Failed)
                throw new InvalidCredentialsException();

            return new AuthResponse
            {
                Email = user.Email,
                Role = user.Role,
                Token = _jwt.GenerateToken(user)
            };
        }
    }
}
