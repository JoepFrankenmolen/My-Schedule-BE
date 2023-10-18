using My_Schedule.AuthService.DTO.Tokens;
using My_Schedule.Shared.DTO.Tokens;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenService
    {
        private readonly IAuthenticationSettings _appSettings;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly TokenSessionService _tokenSessionService;

        public TokenService(IAuthenticationSettings appSettings, ITokenValidator tokenValidator, TokenGenerator tokenGenerator, TokenSessionService tokenSessionService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _tokenSessionService = tokenSessionService ?? throw new ArgumentNullException(nameof(tokenSessionService));
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            var validatedTokenUserDTO = await ValidateToken(refreshToken, TokenType.Refresh);

            if (validatedTokenUserDTO != null && validatedTokenUserDTO.user != null)
            {
                return await GenerateToken(validatedTokenUserDTO.user.Id, TokenType.Access, validatedTokenUserDTO.SessionId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<TokenDTO> CreateTokenDTO(User user)
        {
            var sessionId = await _tokenSessionService.GenerateSession();

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var tokenDTO = new TokenDTO
            {
                AccessTokenExpirationTimestamp = currentTime + _appSettings.AccessTokenExpirationTime,
                AccessToken = await GenerateToken(user.Id, TokenType.Access, sessionId),
                RefreshTokenExpirationTimestamp = currentTime + _appSettings.RefreshTokenExpirationTime,
                RefreshToken = await GenerateToken(user.Id, TokenType.Refresh, sessionId),
            };

            return tokenDTO;
        }

        public async Task<string> GenerateToken(Guid userId, TokenType type, Guid sessionId)
        {
            int expirationtime = 0;

            if (type == TokenType.Access)
            {
                expirationtime = _appSettings.AccessTokenExpirationTime;
            }
            else
            {
                expirationtime = _appSettings.RefreshTokenExpirationTime;
            }

            return await _tokenGenerator.GenerateToken(userId, expirationtime, type, sessionId);
        }

        public async Task<ValidatedTokenUserDTO> ValidateToken(string token, TokenType type)
        {
            return await _tokenValidator.ValidateToken(token, type);
        }
    }
}