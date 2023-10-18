using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.Models.Confirmations;

namespace My_Schedule.AuthService.Services.Confirmations
{
    public class ConfirmationDispatcher
    {
        private readonly EmailConfirmationService _emailConfirmationService;
        private readonly LoginVerificationService _loginVerificationService;
        private readonly PasswordResetService _passwordResetService;

        public ConfirmationDispatcher(EmailConfirmationService emailConfirmationService, PasswordResetService passwordResetService, LoginVerificationService loginVerificationService)
        {
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
            _loginVerificationService = loginVerificationService ?? throw new ArgumentNullException(nameof(loginVerificationService));
            _passwordResetService = passwordResetService ?? throw new Exception(nameof(passwordResetService));
        }

        public async Task<object> DispatchConfirmation(ConfirmDTO confirmDTO)
        {
            // Depending on type decides which kind of confirmation it is.
            switch (confirmDTO.ConfirmationType)
            {
                case ConfirmationType.EmailConfirmation:
                    await _emailConfirmationService.ConfirmEmailConfirmation(confirmDTO);
                    return string.Empty;

                case ConfirmationType.LoginVerification:
                    return await _loginVerificationService.ConfirmLoginVerification(confirmDTO);

                case ConfirmationType.PasswordReset:
                    await _passwordResetService.ConfirmPasswordReset(confirmDTO);
                    return string.Empty;

                default:
                    return string.Empty;
            }
        }
    }
}