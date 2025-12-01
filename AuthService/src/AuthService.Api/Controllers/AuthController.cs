using System.Security.Claims;
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
        private readonly IAuthService _auth;
        private readonly IWebHostEnvironment _env;

        public AuthController(IAuthService auth, IWebHostEnvironment env)
        {
            _auth = auth;
            _env = env;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var result = await _auth.RegisterAsync(req);

            SetJwtCookie(result.Token);

            return Ok(result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _auth.LoginAsync(req);

            SetJwtCookie(result.Token);

            return Ok(result.User);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _auth.GetUserByEmailAsync(email);
            if (user == null)
                return Unauthorized();

            return Ok(user); // returns user info
        }

        private void SetJwtCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // secure in production
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(1),
                Path = "/",
            };
            Response.Cookies.Append("jwt", token, cookieOptions);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() => Ok(await _auth.GetAllUsersAsync());

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Auth Service is working!");

        [Authorize]
        [HttpGet("secure")]
        public async Task<IActionResult> ProtectedRoute() =>
            Ok("Secure Service is Working Good with JWT!");
    }
}
