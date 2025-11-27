using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VideoService.Services
{
    public interface IJoinTokenService
    {
        string GenerateJoinToken(Guid appointmentId, Guid userId, string role, TimeSpan? ttl = null);
        ClaimsPrincipal? ValidateJoinToken(string token);
    }

    public class JoinTokenService : IJoinTokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly int _defaultMinutes;

        public JoinTokenService(IConfiguration config)
        {
            _config = config;
            var secret = _config["VideoService:JoinTokenSecret"] ?? throw new InvalidOperationException("JoinTokenSecret missing");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            _defaultMinutes = int.TryParse(_config["VideoService:JoinTokenExpiryMinutes"], out var m) ? m : 30;
        }

        public string GenerateJoinToken(Guid appointmentId, Guid userId, string role, TimeSpan? ttl = null)
        {
            var expires = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromMinutes(_defaultMinutes));
            var claims = new[]
            {
                new Claim("appointmentId", appointmentId.ToString()),
                new Claim("userId", userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateJoinToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
