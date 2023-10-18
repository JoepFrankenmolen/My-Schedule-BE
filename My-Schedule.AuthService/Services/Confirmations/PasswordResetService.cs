using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Models.PasswordReset;
using My_Schedule.AuthService.Services.Auth;
using My_Schedule.AuthService.Services.Notifications;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Helpers.Validators;
using My_Schedule.Shared.Interfaces.AppSettings;

namespace My_Schedule.AuthService.Services.Confirmations
{
    public class PasswordResetService
    {
        private readonly IUserSettings _appSettings;
        private readonly ConfirmationService _confirmationService;
        private readonly UserHelper _userHelper;
        private readonly HashService _hashService;
        private readonly NotificationTriggerService _notificationTriggerService;
        private readonly AuthServiceContext _dbContext;

        public PasswordResetService(ConfirmationService confirmationService, IUserSettings appSettings, UserHelper userHelper, AuthServiceContext dbContext, HashService hashService, NotificationTriggerService notificationTriggerService)
        {
            _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _notificationTriggerService = notificationTriggerService ?? throw new ArgumentNullException(nameof(notificationTriggerService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task CreatePasswordReset(CredentialsDTO credentialsDTO)
        {
            // validate password before fetching the user
            if (PasswordValidator.IsValidPassword(credentialsDTO.Password))
            {
                var user = await _userHelper.GetUserByEmail(credentialsDTO.Email);

                if (user != null && UserValidator.IsValidUser(user, _appSettings.MaxLoginAttempts))
                {
                    var confirmationDTO = await _confirmationService.CreateConfirmation(user.Id, ConfirmationCodeType.GUID, ConfirmationType.PasswordReset);

                    if (confirmationDTO != null)
                    {
                        // send email
                        _notificationTriggerService.SendPasswordReset(user.Email, confirmationDTO.Confirmation.Id, confirmationDTO.Code);

                        var hashDTO = await _hashService.GenerateSaltAndHash(credentialsDTO.Password);

                        var passwordReset = new PasswordReset
                        {
                            ConfirmationId = confirmationDTO.Confirmation.Id,
                            UserId = user.Id,
                            PasswordHash = hashDTO.PasswordHash,
                            Salt = hashDTO.Salt,
                        };

                        await _dbContext.PasswordResets.AddAsync(passwordReset);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task ConfirmPasswordReset(ConfirmDTO confirmDTO)
        {
            var confirmation = await _confirmationService.ValidateConfirmation(confirmDTO);
            var user = confirmation.User;

            if (user != null)
            {
                if (UserValidator.IsValidUser(user, _appSettings.MaxLoginAttempts))
                {
                    var passwordReset = await GetPasswordReset(confirmation.UserId, confirmation.Id);

                    user.PasswordHash = passwordReset.PasswordHash;
                    user.Salt = passwordReset.Salt;
                    user.TokenRevocationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }

                await _dbContext.SaveChangesAsync();
                return;
            }
            throw new UnauthorizedAccessException();
        }

        private async Task<PasswordReset> GetPasswordReset(Guid userId, Guid confirmationId)
        {
            return await _dbContext.PasswordResets
                .Include(r => r.User)
                .Where(c => c.User.Id == userId && c.ConfirmationId == confirmationId)
                .FirstOrDefaultAsync();
        }
    }
}