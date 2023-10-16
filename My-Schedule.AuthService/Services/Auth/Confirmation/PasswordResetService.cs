﻿using Microsoft.EntityFrameworkCore;
using SecureLogin.Data.Context;
using SecureLogin.Data.DTO.Auth.Authentication;
using SecureLogin.Data.Enums;
using SecureLogin.Data.Models.Confirmations;
using SecureLogin.Data.Models.PasswordReset;
using SecureLogin.Services.Common.Validators;
using SecureLogin.Services.Helpers;
using SecureLogin.Services.Helpers.Validators;
using SecureLogin.Services.Services.ApplicationUsers.Helpers;
using SecureLogin.Services.Services.Notifications;

namespace My_Schedule.AuthService.Services.Auth.Confirmation
{
    public class PasswordResetService
    {
        private readonly IServicesAppSettings _appSettings;
        private readonly ConfirmationService _confirmationService;
        private readonly UserHelper _userHelper;
        private readonly HashService _hashService;
        private readonly NotificationTriggerService _notificationTriggerService;
        private readonly SecureLoginContext _dbContext;

        public PasswordResetService(ConfirmationService confirmationService, IServicesAppSettings appSettings, UserHelper userHelper, SecureLoginContext dbContext, HashService hashService, NotificationTriggerService notificationTriggerService)
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

                        await _dbContext.passwordResets.AddAsync(passwordReset);
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
                    user.RevocationTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }

                await _dbContext.SaveChangesAsync();
                return;
            }
            throw new UnauthorizedAccessException();
        }

        private async Task<PasswordReset> GetPasswordReset(Guid userId, Guid confirmationId)
        {
            return await _dbContext.passwordResets
                .Include(r => r.User)
                .Where(c => c.User.Id == userId && c.ConfirmationId == confirmationId)
                .FirstOrDefaultAsync();
        }
    }
}
