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

        public async Task<AuthResult> RegisterAsync(RegisterRequest req)
        {
            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null)
                throw new EmailAlreadyExistsException();

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Role = req.Role,
            };

            user.PasswordHash = _hasher.HashPassword(user, req.Password);
            await _repo.AddAsync(user);

            var token = _jwt.GenerateToken(user);

            return new AuthResult
            {
                User = new AuthResponse { Email = user.Email, Role = user.Role },
                Token = token,
            };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest req)
        {
            var user =
                await _repo.GetByEmailAsync(req.Email) ?? throw new InvalidCredentialsException();
            var verification = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (verification == PasswordVerificationResult.Failed)
                throw new InvalidCredentialsException();

            var token = _jwt.GenerateToken(user);

            return new AuthResult
            {
                User = new AuthResponse { Email = user.Email, Role = user.Role },
                Token = token,
            };
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                return null;

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
            };
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var users = await _repo.GetByRoleAsync("patient");

            return users
                .Select(u => new PatientDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.Phone,
                    DateOfBirth = u.DateOfBirth,
                })
                .ToList();
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var users = await _repo.GetByRoleAsync("doctor");

            return users
                .Select(u => new DoctorDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Specialization = u.Specialization,
                    Experience = u.Experience,
                })
                .ToList();
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(Guid id)
        {
            var doctor = await _repo.GetDoctorByIdAsync(id);

            if (doctor == null)
                return null;

            return new DoctorDto
            {
                UserId = doctor.UserId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                Experience = doctor.Experience,
            };
        }

        public async Task<PatientDto?> GetPatientByIdAsync(Guid id)
        {
            var patient = await _repo.GetPatientByIdAsync(id);

            if (patient == null)
                return null;

            return new PatientDto
            {
                UserId = patient.UserId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Phone = patient.Phone,
                DateOfBirth = patient.DateOfBirth,
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllAsync();

            return users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Role = u.Role,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.Phone,
                    DateOfBirth = u.DateOfBirth,
                })
                .ToList();
        }
    }
}
