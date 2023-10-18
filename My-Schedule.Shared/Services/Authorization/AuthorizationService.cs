using My_Schedule.Shared.DTO.Tokens;
using My_Schedule.Shared.Helpers;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using My_Schedule.Shared.Services.Tokens;

namespace My_Schedule.Shared.Services.Authorization
{
    public class AuthorizationService
    {
        private readonly TokenValidator _tokenValidator;

        public AuthorizationService(TokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
        }

        public async Task<IUserBasic> AuthorizeRequest(HttpRequest request)
        {
            var token = HttpContextHelper.GetBearerToken(request) ?? throw new ArgumentNullException();

            // validate the token, this includes the user stuff.
            var validatedTokenUserDTO = await _tokenValidator.ValidateToken(token, TokenType.Access);

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