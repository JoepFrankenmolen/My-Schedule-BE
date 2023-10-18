using SecureLogin.Data.DTO.Auth.Authentication;
using SecureLogin.Data.Enums;
using SecureLogin.Services.Services.Notifications;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Data.Models.Confirmations;
using My_Schedule.AuthService.Context;

namespace My_Schedule.AuthService.Services.Auth.Confirmation
{
    public class EmailConfirmationService
    {
        private readonly ConfirmationService _confirmationService;
        private readonly NotificationTriggerService _notificationTriggerService;
        private readonly SecureLoginContext _dbContext;

        public EmailConfirmationService(ConfirmationService confirmationService, SecureLoginContext dbContext, NotificationTriggerService notificationTriggerService)
        {
            _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
            _notificationTriggerService = notificationTriggerService ?? throw new ArgumentNullException(nameof(notificationTriggerService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Guid> CreateEmailConfirmation(User user)
        {
            var confirmationDTO = await _confirmationService.CreateConfirmation(user.Id, ConfirmationCodeType.INT, ConfirmationType.EmailConfirmation);
            _notificationTriggerService.SendEmailConfirmation(user.Email, confirmationDTO.Code);

            return confirmationDTO.Confirmation.Id;
        }

        public async Task ConfirmEmailConfirmation(ConfirmDTO confirmDTO)
        {
            var confirmation = await _confirmationService.ValidateConfirmation(confirmDTO);

            if (confirmation != null)
            {
                confirmation.User.IsEmailConfirmed = true;
                confirmation.User.RevocationTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                await _dbContext.SaveChangesAsync();
                return;
            }
            throw new UnauthorizedAccessException();
        }
    }
}
