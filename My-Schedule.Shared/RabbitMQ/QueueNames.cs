namespace My_Schedule.Shared.RabbitMQ
{
    public static class QueueNames
    {
        public static class Users
        {
            public const string UserBlocked = "user.blocked";
            public const string UserBanned = "user.banned";
            public const string UserTokenRevocation = "user.token_revocation";
            public const string UserEmailConfirmation = "user.email_confirmation";
            public const string UserIdentityUpdate = "user.identity_update";
            public const string UserRoleUpdate = "user.role_update";
            public const string UserCreated = "user.created";
            public const string UserDeleted = "user.deleted";
        }

        public static class UserActivity
        {
            public const string SuccessfullLogin = "userActivity.successfull_login";
            public const string FailedLoginAttempt = "userActivity.failed_login_attempt";
        }

        public static class UserSettings
        {
            public const string TwoFactorEnabledUpdate = "userSettings.two_factor_enabled_update";
        }

        public static class Tokens
        {
            public const string TokenStatusCreated = "tokenStatus.created";
        }

        public static class Notifications
        {
            public const string Notificationtriggered = "notification.triggered";
        }
    }
}