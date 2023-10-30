using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Services.Notifications;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.AuthService.Services.Confirmations
{
    public class EmailConfirmationService
    {
        private readonly ConfirmationService _confirmationService;
        private readonly NotificationTriggerService _notificationTriggerService;
        private readonly AuthServiceContext _dbContext;
        private readonly IUserUpdateService _userUpdateService;


        public EmailConfirmationService(
            ConfirmationService confirmationService,
            AuthServiceContext dbContext,
            NotificationTriggerService notificationTriggerService,
            IUserUpdateService userUpdateService)
        {
            _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
            _notificationTriggerService = notificationTriggerService ?? throw new ArgumentNullException(nameof(notificationTriggerService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
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

            if (confirmation == null)
            {
                throw new UnauthorizedAccessException();
            }

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            await _userUpdateService.EmailConfirmation(confirmation.UserId, true, timestamp, _dbContext);
            // place the token generation here.
        }
    }
}