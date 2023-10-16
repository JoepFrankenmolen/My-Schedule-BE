using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using My_Schedule.AuthService.Services.Auth.Tokens;
using SecureLogin.Data.DTO.Auth.Tokens;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Common.Helpers;

namespace My_Schedule.AuthService.Services.Auth.Authorization
{
    public class AuthorizationService
    {
        private readonly TokenService _tokenService;

        public AuthorizationService(TokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<User> AuthorizeRequest(HttpRequest request)
        {
            var token = HttpContextHelper.GetBearerToken(request) ?? throw new ArgumentNullException();

            // validate the token, this includes the user stuff.
            var validatedTokenUserDTO = await _tokenService.ValidateToken(token, TokenType.Access);

            if (validatedTokenUserDTO == null)
            {
                return null;
            }
            else
            {
                return validatedTokenUserDTO.user;
            }
        }
    }
}
