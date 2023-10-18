using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Tokens;
using My_Schedule.AuthService.Models;
using My_Schedule.AuthService.Services.Auth.Tokens;

namespace My_Schedule.AuthService.Services.Auth.Confirmation
{
    public class LoginVerificationService
    {
        private readonly ConfirmationService _confirmationService;
        private readonly NotificationTriggerService _notificationTriggerService;
        private readonly TokenService _tokenService;

        public LoginVerificationService(ConfirmationService confirmationService, NotificationTriggerService notificationTriggerService, TokenService tokenService)
        {
            _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
            _notificationTriggerService = notificationTriggerService ?? throw new ArgumentNullException(nameof(notificationTriggerService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<Guid> CreateLoginVerification(User user)
        {
            var confirmationDTO = await _confirmationService.CreateConfirmation(user.Id, ConfirmationCodeType.INT, ConfirmationType.LoginVerification);
            _notificationTriggerService.SendLoginVerification(user.Email, confirmationDTO.Code);

            return confirmationDTO.Confirmation.Id;
        }

        public async Task<TokenDTO> ConfirmLoginVerification(ConfirmDTO confirmDTO)
        {
            var confirmation = await _confirmationService.ValidateConfirmation(confirmDTO);

            if (confirmation != null)
            {
                return await _tokenService.CreateTokenDTO(confirmation.User);
            }
            throw new UnauthorizedAccessException();
        }
    }
}
