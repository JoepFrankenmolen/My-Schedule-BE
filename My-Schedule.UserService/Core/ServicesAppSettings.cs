using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;

namespace My_Schedule.UserService.Core
{
    public class ServicesAppSettings : IDatabaseSettings, IAuthenticationSettings, IEmailSettings
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
        public string SenderEmail => _appSettings.SenderEmail;
        public string SenderPassword => _appSettings.SenderPassword;
    }
}