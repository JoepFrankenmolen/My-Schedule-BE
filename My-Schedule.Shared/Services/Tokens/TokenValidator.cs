using Microsoft.IdentityModel.Tokens;
using My_Schedule.Shared.DTO.Tokens;
using My_Schedule.Shared.Helpers.Validators;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users.UserInterfaces;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using My_Schedule.Shared.Services.Users.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My_Schedule.Shared.Services.Tokens
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IAuthenticationSettings _appSettings;
        private readonly ITokenSessionValidator _tokenSessionValidator;
        private readonly IUserContext _dbContext;

        public TokenValidator(
            IAuthenticationSettings appSettings,
            ITokenSessionValidator tokenSessionService,
            IUserContext dbContext)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _tokenSessionValidator = tokenSessionService ?? throw new ArgumentNullException(nameof(tokenSessionService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                var user = await UserFetcherService.GetUserById(userId, _dbContext);

                // check if the user is blocked or token is revoked.
                if (user == null || !UserValidator.IsValidUser(user) || await IsTokenRevoked(user, securityToken, sessionId))
                {
                    return null;
                }

                return new ValidatedTokenUserDTO
                {
                    user = user,
                    SessionId = sessionId,
                };
            }
            catch (Exception ex)
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

        // Checks if token is revoked based on timestamps and if the session is valid.
        private async Task<bool> IsTokenRevoked(IUserStatus user, SecurityToken token, Guid sessionId)
        {
            long tokenValidFromUnixTimestamp = ConvertDateTimeToUnixTimestamp(token.ValidFrom);

            if (user.TokenRevocationTimestamp >= tokenValidFromUnixTimestamp || !await _tokenSessionValidator.IsValidSession(sessionId))
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