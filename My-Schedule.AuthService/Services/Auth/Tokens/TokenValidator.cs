using Microsoft.IdentityModel.Tokens;
using SecureLogin.Data.DTO.Auth.Tokens;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Common.Validators;
using SecureLogin.Services.Helpers;
using SecureLogin.Services.Services.ApplicationUsers.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenValidator
    {
        private readonly IServicesAppSettings _appSettings;
        private readonly UserHelper _userHelper;
        private readonly TokenSessionService _tokenSessionService;

        public TokenValidator(IServicesAppSettings appSettings, UserHelper userHelper, TokenSessionService tokenSessionService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _tokenSessionService = tokenSessionService ?? throw new ArgumentNullException(nameof(tokenSessionService));
        }

        public async Task<ValidatedTokenUserDTO> ValidateToken(string token, TokenType type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.JWTSigningKey);

            try
            {
                // validate token fields
                var securityToken = ValidateToken(tokenHandler, token, key, type);

                // get guid's form the token
                var userId = ExtractGuidFromToken(securityToken, CustomClaimTypes.UserId);
                var sessionId = ExtractGuidFromToken(securityToken, CustomClaimTypes.SessionId);

                // get user;
                var user = await _userHelper.GetUserById(userId);

                // check if the user is blocked or token is revoked.
                if (user == null || !UserValidator.IsValidUser(user, _appSettings.MaxLoginAttempts) || await IsTokenRevoked(user, securityToken, sessionId))
                {
                    return null;
                }

                return new ValidatedTokenUserDTO
                {
                    user = user,
                    SessionId = sessionId,
                };
            }
            catch
            {
                return null;
            }
        }

        // returns null if not valid
        private SecurityToken ValidateToken(JwtSecurityTokenHandler tokenHandler, string token, byte[] key, TokenType type)
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _appSettings.JWTIssuer,
                ValidAudience = _appSettings.JWTAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var claim = ExtractClaimFromToken(validatedToken, CustomClaimTypes.TokenType.ToString()); ////should be tested asap
            if (claim.Value == type.ToString())
            {
                return validatedToken;
            }

            return null;
        }

        // alter this to check the session revoced section
        private async Task<bool> IsTokenRevoked(User user, SecurityToken token, Guid sessionId)
        {
            long tokenValidFromUnixTimestamp = ConvertDateTimeToUnixTimestamp(token.ValidFrom);

            if (user.RevocationTimeStamp >= tokenValidFromUnixTimestamp || !await _tokenSessionService.isValidSession(sessionId))
            {
                return true; // Token is revoked
            }

            return false; // Token is not revoked
        }

        private long ConvertDateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dateTime.ToUniversalTime() - unixEpoch;
            return (long)timeSpan.TotalSeconds;
        }

        private Guid ExtractGuidFromToken(SecurityToken token, string type)
        {
            var claim = ExtractClaimFromToken(token, type);
            if (claim != null && Guid.TryParse(claim.Value, out Guid userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Guid could not be extracted from the token.");
        }

        private Claim ExtractClaimFromToken(SecurityToken token, string type)
        {
            if (token is JwtSecurityToken jwtToken)
            {
                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == type);
                return claim;
            }
            return null;
        }
    }
}
