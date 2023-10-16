using SecureLogin.Data.DTO.Auth.Tokens;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Helpers;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenService
    {
        private readonly IServicesAppSettings _appSettings;
        private readonly TokenGenerator _tokenGenerator;
        private readonly TokenValidator _tokenValidator;
        private readonly TokenSessionService _tokenSessionService;

        public TokenService(IServicesAppSettings appSettings, TokenValidator tokenValidator, TokenGenerator tokenGenerator, TokenSessionService tokenSessionService)
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
                return await GenerateToken(validatedTokenUserDTO.user, TokenType.Access, validatedTokenUserDTO.SessionId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<TokenDTO> CreateTokenDTO(User user)
        {
            var sessionId = await _tokenSessionService.GenerateSession();

            var tokenDTO = new TokenDTO
            {
                AccessToken = await GenerateToken(user, TokenType.Access, sessionId),
                RefreshToken = await GenerateToken(user, TokenType.Refresh, sessionId),
            };

            return tokenDTO;
        }

        public async Task<string> GenerateToken(User user, TokenType type, Guid sessionId)
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

            return await _tokenGenerator.GenerateToken(user, expirationtime, type, sessionId);
        }

        public async Task<ValidatedTokenUserDTO> ValidateToken(string token, TokenType type)
        {
            return await _tokenValidator.ValidateToken(token, type);
        }
    }
}
