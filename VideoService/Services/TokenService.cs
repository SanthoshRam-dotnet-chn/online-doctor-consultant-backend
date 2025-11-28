using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VideoService.Services
{
    public interface IJoinTokenService
    {
        string GenerateJoinToken(Guid appointmentId, Guid userId, string role);
        ClaimsPrincipal? Validate(string token);
    }

    public class JoinTokenService : IJoinTokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public JoinTokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["VideoService:JoinTokenSecret"]));
        }

        public string GenerateJoinToken(Guid appointmentId, Guid userId, string role)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["VideoService:JoinTokenExpiryMinutes"]));

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("appointmentId", appointmentId.ToString()),
                    new Claim("userId", userId.ToString()),
                    new Claim(ClaimTypes.Role, role)
                },
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? Validate(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                return handler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = _key,
                        ValidateLifetime = true
                    },
                    out _
                );
            }
            catch
            {
                return null;
            }
        }
    }
}
