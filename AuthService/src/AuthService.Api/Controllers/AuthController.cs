using AuthService.src.AuthService.Application.DTOs;
using AuthService.src.AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.src.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            Ok(await _service.RegisterAsync(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) =>
            Ok(await _service.LoginAsync(request));

        [Authorize]
        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients() =>
            Ok(await _service.GetAllPatientsAsync());

        [Authorize]
        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors() => Ok(await _service.GetAllDoctorsAsync());

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() => Ok(await _service.GetAllUsersAsync());

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Auth Service is working!");

        [Authorize]
        [HttpGet("secure")]
        public async Task<IActionResult> ProtectedRoute() => Ok("Auth Service is working!");
    }
}
