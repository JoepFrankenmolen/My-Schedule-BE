using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Services.Confirmations;
using My_Schedule.AuthService.Services.Logs;
using My_Schedule.Shared.Helpers.Validators;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.AuthService.Services.Auth.Authentication
{
    public class LoginService
    {
        private readonly LoginVerificationService _loginVerificationService;
        private readonly EmailConfirmationService _emailConfirmationService;
        private readonly IUserSettings _appSettings;
        private readonly HashService _hashService;
        private readonly IUserAuthDetailHelper _userAuthDetailHelper;
        private readonly AuthServiceContext _dbContext;
        private readonly LoginLogService _loginLogService;

        public LoginService(AuthServiceContext dbContext, LoginVerificationService loginVerificationService, EmailConfirmationService emailConfirmationService, IUserSettings appSettings, IUserAuthDetailHelper userHelper, HashService hashService, LoginLogService loginLogService)
        {
            _loginVerificationService = loginVerificationService ?? throw new ArgumentNullException(nameof(loginVerificationService));
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
            _userAuthDetailHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _loginLogService = loginLogService ?? throw new ArgumentNullException(nameof(loginLogService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ConfirmationCodeResponse> Login(CredentialsDTO credentialsDTO)
        {
            // Get the user. Can be null.
            var userAuth = await _userAuthDetailHelper.GetUserByEmail(credentialsDTO.Email, _dbContext);
            var response = new ConfirmationCodeResponse();
            var maxAttempts = _appSettings.MaxLoginAttempts;
            var isValid = false;

            // checks if the user exists.
            if (userAuth != null && userAuth.User != null)
            {
                // validate the user and if correct generate a token
                if (UserValidator.IsValidUser(userAuth, maxAttempts, false) && await ValidatePasswordHash(userAuth, credentialsDTO))
                {
                    // temporary here bc no clear way yet to communicate the email is not confirmed yet!!!!!!!!!!!!!!!!!
                    if (userAuth.User.IsEmailConfirmed == false)
                    {
                        var id = await _emailConfirmationService.CreateEmailConfirmation(userAuth.User);

                        response = CreateResponse(response, id, ConfirmationType.EmailConfirmation);
                    }
                    else
                    {
                        // Update user fields due to SuccessFullLogin.
                        userAuth = await _userAuthDetailHelper.UpdateOnLoginSuccess(userAuth, _dbContext);

                        // Set isValid to true.
                        var id = await _loginVerificationService.CreateLoginVerification(userAuth.User);

                        response = CreateResponse(response, id, ConfirmationType.LoginVerification);
                    }
                    isValid = true;
                }
                else
                {
                    await _userAuthDetailHelper.UpdateOnLoginFail(userAuth, maxAttempts, _dbContext);
                }
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

        private async Task<bool> ValidatePasswordHash(UserAuthDetail userAuth, CredentialsDTO credentialsDTO)
        {
            var passwordHash = await _hashService.GenerateHash(credentialsDTO.Password, userAuth.Salt);

            if (userAuth.PasswordHash == passwordHash)
            {
                return true;
            }
            return false;
        }

        private ConfirmationCodeResponse CreateResponse(ConfirmationCodeResponse response, Guid confirmationId, ConfirmationType type)
        {
            response.ConfirmationId = confirmationId.ToString();
            response.CharachterType = ConfirmationCodeType.INT;
            response.Type = type;

            return response;
        }
    }
}