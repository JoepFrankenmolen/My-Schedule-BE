namespace My_Schedule.Shared.Core
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string DatabaseConnection => LoadSetting("DatabaseConnection");
        public string JWTIssuer => LoadSetting("JWTIssuer");
        public string JWTAudience => LoadSetting("JWTAudience");
        public string JWTSigningKey => LoadSetting("JWTSigningKey");
        public int AccessTokenExpirationTime => int.Parse(LoadSetting("AccessTokenExpirationTime"));
        public int RefreshTokenExpirationTime => int.Parse(LoadSetting("RefreshTokenExpirationTime"));
        public int ConfirmationExpirationTime => int.Parse(LoadSetting("ConfirmationExpirationTime"));
        public int MaxLoginAttempts => int.Parse(LoadSetting("MaxLoginAttempts"));
        public int MaxConfirmationAttempts => int.Parse(LoadSetting("MaxConfirmationAttempts"));
        public string Pepper => LoadSetting("Pepper");
        public string SenderEmail => LoadSetting("SenderEmail");
        public string SenderPassword => LoadSetting("SenderPassword");

        private string LoadSetting(string key)
        {
            var value = _configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                throw new FormatException($"AppSetting: {key} is empty");
            }

            return value;
        }
    }
}