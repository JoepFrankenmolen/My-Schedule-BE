using Microsoft.IdentityModel.Tokens;
using SecureLogin.Data.DTO.Auth.Tokens;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenGenerator
    {
        private readonly IServicesAppSettings _appSettings;

        public TokenGenerator(IServicesAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<string> GenerateToken(User user, int tokenExpirationTime, TokenType type, Guid sessionId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.JWTSigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        // add more claims here is you want to increase the jwt tokens information
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(CustomClaimTypes.SessionId, sessionId.ToString()),
                        new Claim(CustomClaimTypes.TokenType, type.ToString()),
                    }),

                Expires = DateTime.UtcNow.AddSeconds(tokenExpirationTime),
                Issuer = _appSettings.JWTIssuer,
                Audience = _appSettings.JWTAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
