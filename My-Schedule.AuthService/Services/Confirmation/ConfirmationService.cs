using SecureLogin.Services.Helpers;
using SecureLogin.Services.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using SecureLogin.Data.DTO.Auth.Authentication;
using SecureLogin.Data.Enums;
using SecureLogin.Services.Services.Loging;
using SecureLogin.Data.Models.Confirmations;
using My_Schedule.AuthService.Context;

namespace My_Schedule.AuthService.Services.Auth.Confirmation
{
    public class ConfirmationService
    {
        private readonly IServicesAppSettings _appSettings;
        private readonly HashService _hashService;
        private readonly AuthServiceContext _dbContext;
        private readonly ConfirmationLogService _confirmationLogService;

        public ConfirmationService(IServicesAppSettings appSettings, HashService hashService, AuthServiceContext dbContext, ConfirmationLogService confirmationLogService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _confirmationLogService = confirmationLogService ?? throw new ArgumentNullException(nameof(confirmationLogService));
        }

        public async Task<ConfirmationDTO> CreateConfirmation(Guid userId, ConfirmationCodeType codeType, ConfirmationType confirmationType)
        {
            var code = GenerateConfirmationCode(codeType);
            var codeHash = await GenerateHash(code);

            var confirmation = new Confirmation
            {
                CreationTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ExpirationTimeStamp = DateTimeOffset.UtcNow.AddSeconds(_appSettings.ConfirmationExpirationTime).ToUnixTimeSeconds(),
                UserId = userId,
                ConfirmationType = confirmationType,
                Code = codeHash,
                IsConfirmed = false,
                IsBlocked = false,
                Attempts = 0,
            };

            await _dbContext.Confirmations.AddAsync(confirmation);
            await _dbContext.SaveChangesAsync();

            return new ConfirmationDTO
            {
                Confirmation = confirmation,
                Code = code
            };
        }

        /// <summary>
        /// Only returns something if it is valid
        /// </summary>
        /// <param name="confirmDTO"></param>
        /// <returns></returns>
        public async Task<Confirmation> ValidateConfirmation(ConfirmDTO confirmDTO)
        {
            var codeHash = await GenerateHash(confirmDTO.Code);
            var isValid = false;
            var userId = new Guid();

            // gets the confirmation email
            var confirmation = await GetConfirmation(confirmDTO.ConfirmationId);

            // check if not null
            if (confirmation != null)
            {
                // set values
                isValid = true;
                userId = confirmation.UserId;

                // validate the confirmation
                if (await ValidateConfirmation(confirmation, codeHash, confirmDTO.ConfirmationType))
                {
                    confirmation.IsConfirmed = true;
                }
                else
                {
                    if (!confirmation.IsBlocked && confirmation.Attempts >= _appSettings.MaxConfirmationAttempts)
                    {
                        confirmation.IsBlocked = true;
                    }

                    // because failed set isValid to false for good measure
                    isValid = false;
                }

                confirmation.Attempts++;

                await _dbContext.SaveChangesAsync();
            }

            await _confirmationLogService.CreateConfirmationLog(userId, confirmDTO.ConfirmationId, !isValid);

            // if not confirmed or valid return null
            if (isValid && confirmation.IsConfirmed)
            {
                return confirmation;
            }
            else
            {
                return null;
            }

        }

        private async Task<bool> ValidateConfirmation(Confirmation confirmation, string codeHash, ConfirmationType confirmationType)
        {
            var maxAttempts = _appSettings.MaxConfirmationAttempts;

            if (confirmation.IsBlocked || confirmation.IsConfirmed || confirmationType != confirmation.ConfirmationType || confirmation.ExpirationTimeStamp < DateTimeOffset.UtcNow.ToUnixTimeSeconds() || confirmation.Attempts >= maxAttempts)
            {
                return false;
            }

            if (confirmation.Code == codeHash)
            {
                confirmation.IsConfirmed = true;

                return true;
            }

            return false;
        }

        private async Task<string> GenerateHash(string code)
        {
            return await _hashService.GenerateHashSha(code);
        }

        private async Task<Confirmation> GetConfirmation(Guid confirmationId)
        {
            return await _dbContext.Confirmations
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == confirmationId);
        }

        private string GenerateConfirmationCode(ConfirmationCodeType type)
        {
            string code = string.Empty;

            switch (type)
            {
                case ConfirmationCodeType.INT:
                    code = RandomNumberGenerator.GenerateRandomNumbers(6).ToString();
                    break;
                case ConfirmationCodeType.GUID:
                    code = Guid.NewGuid() + Guid.NewGuid().ToString();
                    break;
            }

            return code;
        }
    }
}
