
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthService.src.AuthService.Application.DTOs;
using AuthService.src.AuthService.Application.Interfaces;


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
