using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;

namespace My_Schedule.AuthService.Core
{
    public class ServicesAppSettings : IDatabaseSettings, IConfirmationSettings, IAuthenticationSettings, IUserSettings
    {
        private readonly AppSettings _appSettings;

        public ServicesAppSettings(AppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public string DatabaseConnection => _appSettings.DatabaseConnection;
        public string JWTIssuer => _appSettings.JWTIssuer;
        public string JWTAudience => _appSettings.JWTAudience;
        public string JWTSigningKey => _appSettings.JWTSigningKey;
        public int AccessTokenExpirationTime => _appSettings.AccessTokenExpirationTime;
        public int RefreshTokenExpirationTime => _appSettings.RefreshTokenExpirationTime;
        public int ConfirmationExpirationTime => _appSettings.ConfirmationExpirationTime;
        public int MaxLoginAttempts => _appSettings.MaxLoginAttempts;
        public int MaxConfirmationAttempts => _appSettings.MaxConfirmationAttempts;
        public string Pepper => _appSettings.Pepper;
        public string SenderEmail => _appSettings.SenderEmail;
        public string SenderPassword => _appSettings.SenderPassword;
    }
}
