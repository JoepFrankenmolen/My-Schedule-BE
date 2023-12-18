using My_Schedule.Shared.DTO.Tokens;
using My_Schedule.Shared.Helpers;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Authorization.Interfaces;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.Shared.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ITokenValidator _tokenValidator;

        public AuthorizationService(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
        }

        public async Task<IUser> AuthorizeRequest(HttpRequest request)
        {
            var token = HttpContextHelper.GetBearerToken(request) ?? throw new ArgumentNullException();

            // validate the token, this includes the user stuff.
            var validatedTokenUserDTO = await _tokenValidator.ValidateToken(token, TokenType.Access);

            if (validatedTokenUserDTO == null)
            {
                throw new ArgumentNullException(nameof(validatedTokenUserDTO));
            }
            return validatedTokenUserDTO.user;
        }
    }
}