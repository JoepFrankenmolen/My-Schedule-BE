namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenDevelopmentGenerator
    {
        private readonly IServicesAppSettings _servicesAppSettings;
        private readonly TokenService _tokenService;
        private readonly UserHelper _userHelper;

        public TokenDevelopmentGenerator(IServicesAppSettings servicesAppSettings, TokenService tokenService, UserHelper userHelper)
        {
            _servicesAppSettings = servicesAppSettings ?? throw new ArgumentNullException(nameof(servicesAppSettings));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
        }

        public async Task<string> GenerateToken()
        {
            var user = await _userHelper.GetUserById(StringToGuid(_servicesAppSettings.LoggedInUserId));

            var tokenDTO = await _tokenService.CreateTokenDTO(user);

            return tokenDTO.AccessToken;
        }

        private Guid StringToGuid(string input)
        {
            Guid result;
            if (Guid.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid GUID format.", nameof(input));
            }
        }
    }
}