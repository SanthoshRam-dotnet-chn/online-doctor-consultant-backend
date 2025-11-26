using JwtTestingDemo.src.AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthService.src.AuthService.Application.DTOs;


namespace UserAuthService.src.AuthService.Api.Controllers
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
        public async Task<IActionResult> Register(RegisterRequest request)
            => Ok(await _service.RegisterAsync(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
            => Ok(await _service.LoginAsync(request));
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok("Auth Service is working!");
    }
}
