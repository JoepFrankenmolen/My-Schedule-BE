using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;

namespace My_Schedule.UserService.Core
{
    public class ServicesAppSettings : IDatabaseSettings, IAuthenticationSettings, IEmailSettings, IMessageQueueSettings
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
        public string MessageQueueHostName => _appSettings.MessageQueueHostName;
        public int MessageQueuePort => _appSettings.MessageQueuePort;
        public string MessageQueueUserName => _appSettings.MessageQueueUserName;
        public string MessageQueuePassword => _appSettings.MessageQueuePassword;
        public string MessageQueueVirtualHost => _appSettings.MessageQueueVirtualHost;
        public bool MessageQueueUseSsl => _appSettings.MessageQueueUseSsl;
        public string MessageQueueHeaderName => _appSettings.MessageQueueHeaderName;
        public string MessageQueueServiceName => _appSettings.MessageQueueServiceName;
    }
}