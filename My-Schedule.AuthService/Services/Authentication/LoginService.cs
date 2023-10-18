using My_Schedule.AuthService.Context;
using My_Schedule.AuthService.Services.Auth.Confirmation;

namespace My_Schedule.AuthService.Services.Auth.Authentication
{
    public class LoginService
    {
        private readonly LoginVerificationService _loginVerificationService;
        private readonly EmailConfirmationService _emailConfirmationService;
        private readonly IServicesAppSettings _appSettings;
        private readonly HashService _hashService;
        private readonly UserHelper _userHelper;
        private readonly SecureLoginContext _dbContext;
        private readonly UserService _userService;
        private readonly LoginLogService _loginLogService;

        public LoginService(SecureLoginContext dbContext, LoginVerificationService loginVerificationService, EmailConfirmationService emailConfirmationService, IServicesAppSettings appSettings, UserHelper userHelper, HashService hashService, UserService userservice, LoginLogService loginLogService)
        {
            _loginVerificationService = loginVerificationService ?? throw new ArgumentNullException(nameof(loginVerificationService));
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _userService = userservice ?? throw new ArgumentNullException(nameof(userservice));
            _loginLogService = loginLogService ?? throw new ArgumentNullException(nameof(loginLogService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<LoginDTO> Login(CredentialsDTO credentialsDTO)
        {
            // get a user can be null
            var user = await _userHelper.GetUserByEmail(credentialsDTO.Email);
            var loginDTO = new LoginDTO();
            var maxAttempts = _appSettings.MaxLoginAttempts;
            var isValid = false;

            // checks if the user exists.
            if (user != null)
            {
                // temporary here bc no clear way yet to communicate the email is not confirmed yet!!!!!!!!!!!!!!!!!

                // validate the user and if correct generate a token
                if (UserValidator.IsValidUser(user, maxAttempts, false) && await ValidatePasswordHash(user, credentialsDTO))
                {
                    if (user.IsEmailConfirmed == false)
                    {
                        var id = await _emailConfirmationService.CreateEmailConfirmation(user);

                        loginDTO.confirmationId = id.ToString();
                        loginDTO.type = "email";
                    }
                    else
                    {
                        // reset AccesFailedCount becuase successfull login attempt
                        if (user.AccessFailedCount != 0)
                        {
                            user.AccessFailedCount = 0;
                            user.LastLoggedInTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        }

                        // Set isValid to true.
                        var id = await _loginVerificationService.CreateLoginVerification(user);

                        loginDTO.confirmationId = id.ToString();
                        loginDTO.type = "login";
                    }
                    isValid = true;
                }
                else
                {
                    if (user.AccessFailedCount >= maxAttempts && user.IsBlocked == false)
                    {
                        user.IsBlocked = true;
                    }

                    user.AccessFailedCount++;
                }

                await _dbContext.SaveChangesAsync();
            }

            // Create a log in the database.
            await _loginLogService.CreateLoginLog(credentialsDTO.Email, !isValid);

            // If the user is valid than send an email with a confirmation code.
            if (isValid)
            {
                return loginDTO;
            }

            throw new UnauthorizedAccessException();
        }

        private async Task<bool> ValidatePasswordHash(User user, CredentialsDTO credentialsDTO)
        {
            var passwordHash = await _hashService.GenerateHash(credentialsDTO.Password, user.Salt);

            if (user.PasswordHash == passwordHash)
            {
                return true;
            }
            return false;
        }
    }
}
