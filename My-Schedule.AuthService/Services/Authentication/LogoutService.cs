using Microsoft.Extensions.Primitives;
using My_Schedule.AuthService.Services.Auth.Tokens;
using My_Schedule.Shared.DTO.Tokens;
using My_Schedule.Shared.Helpers;

namespace My_Schedule.AuthService.Services.Authentication
{
    public class LogoutService
    {
        private readonly TokenService _tokenService;
        private readonly TokenSessionService _tokenSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutService(TokenService tokenService, TokenSessionService tokenSessionService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _tokenSessionService = tokenSessionService ?? throw new ArgumentNullException(nameof(tokenSessionService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> Logout()
        {
            var token = HttpContextHelper.GetBearerToken(_httpContextAccessor.HttpContext.Request);

            if (!StringValues.IsNullOrEmpty(token))
            {
                var validatedTokenDTO = await _tokenService.ValidateToken(token, TokenType.Access);

                if (validatedTokenDTO != null)
                {
                    if (await _tokenSessionService.BlockSession(validatedTokenDTO.SessionId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}