using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Services.Confirmations;
using My_Schedule.AuthService.Services.Logs;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Helpers.Validators;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.AuthService.Services.Auth.Authentication
{
    public class LoginService
    {
        private readonly LoginVerificationService _loginVerificationService;
        private readonly EmailConfirmationService _emailConfirmationService;
        private readonly IUserSettings _appSettings;
        private readonly HashService _hashService;
        private readonly UserHelper _userHelper;
        private readonly AuthServiceContext _dbContext;
        private readonly LoginLogService _loginLogService;

        public LoginService(AuthServiceContext dbContext, LoginVerificationService loginVerificationService, EmailConfirmationService emailConfirmationService, IUserSettings appSettings, UserHelper userHelper, HashService hashService, LoginLogService loginLogService)
        {
            _loginVerificationService = loginVerificationService ?? throw new ArgumentNullException(nameof(loginVerificationService));
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _loginLogService = loginLogService ?? throw new ArgumentNullException(nameof(loginLogService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ConfirmationCodeResponse> Login(CredentialsDTO credentialsDTO)
        {
            // Get the user. Can be null.
            var user = await _userHelper.GetUserByEmail(credentialsDTO.Email);
            var response = new ConfirmationCodeResponse();
            var maxAttempts = _appSettings.MaxLoginAttempts;
            var isValid = false;

            // checks if the user exists.
            if (user != null)
            {
                // validate the user and if correct generate a token
                if (UserValidator.IsValidUser(user, maxAttempts, false) && await ValidatePasswordHash(user, credentialsDTO))
                {
                    // temporary here bc no clear way yet to communicate the email is not confirmed yet!!!!!!!!!!!!!!!!!
                    if (user.IsEmailConfirmed == false)
                    {
                        var id = await _emailConfirmationService.CreateEmailConfirmation(user);

                        response.ConfirmationId = id.ToString();
                        response.CharachterType = ConfirmationCodeType.INT;
                        response.Type = ConfirmationType.EmailConfirmation;
                    }
                    else
                    {
                        // reset AccesFailedCount because successfull login attempt
                        if (user.FailedLoginAttempts != 0)
                        {
                            user.FailedLoginAttempts = 0;
                            user.LastLoginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        }

                        // Set isValid to true.
                        var id = await _loginVerificationService.CreateLoginVerification(user);

                        response.ConfirmationId = id.ToString();
                        response.CharachterType = ConfirmationCodeType.INT;
                        response.Type = ConfirmationType.LoginVerification;
                    }
                    isValid = true;
                }
                else
                {
                    if (user.FailedLoginAttempts >= maxAttempts && user.IsBlocked == false)
                    {
                        user.IsBlocked = true;
                    }

                    user.FailedLoginAttempts++;
                }

                await _dbContext.SaveChangesAsync();
            }

            // Create a log in the database.
            await _loginLogService.CreateLoginLog(credentialsDTO.Email, !isValid);

            // If the user is valid than send an email with a confirmation code.
            if (isValid)
            {
                return response;
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